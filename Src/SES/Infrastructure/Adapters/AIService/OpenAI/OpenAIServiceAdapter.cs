using Application.Services.AIService;
using Application.Services.AIService.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Schema.Generation;
using OpenAI;
using OpenAI.Chat;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Adapters.AIService.OpenAI;

public class OpenAIServiceAdapter : IAIService
{
    private readonly OpenAIClient _aIClient;
    private static readonly JsonSerializerOptions DefaultJsonOptions = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };

    public OpenAIServiceAdapter(IConfiguration configuration)
    {
        _aIClient = new(configuration.GetSection("OpenAiApiKey").Get<string>());
    }

    public async Task<List<AIQuestionGenerationResponse>> GenerateQuestionsFromAIAsync(AIQuestionGenerationRequest generationRequest, string aiModel, CancellationToken cancellationToken)
    {
        List<ChatMessage> chatMessages = await GetChatMessages(generationRequest, "QuestionPrompt.txt");

        ChatClient chatClient = _aIClient.GetChatClient(aiModel);

        JSchemaGenerator schemaGenerator = new();

        string jsonSchema = schemaGenerator.Generate(typeof(AIQuestionGenerationResponseList)).ToString();

        int tokenLimit = Math.Min(generationRequest.QuestionCount * 150 + 500, 4096);

        ChatCompletionOptions completionOptions = new()
        {
            Temperature = 1f,
            MaxOutputTokenCount = tokenLimit,
            TopP = 1,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(jsonSchemaFormatName: "QuizQuestion",
                                                                       jsonSchema: BinaryData.FromString(jsonSchema),
                                                                       jsonSchemaIsStrict: true)
        };

        ChatCompletion? chat = await chatClient.CompleteChatAsync(messages: chatMessages,
                                                                  options: completionOptions,
                                                                  cancellationToken: cancellationToken);
        RecordAsync(generationRequest, aiModel, chat);

        if (string.IsNullOrWhiteSpace(chat.Content[0].Text))
            throw new InvalidOperationException("AI returned empty response");

        AIQuestionGenerationResponseList questions = JsonSerializer.Deserialize<AIQuestionGenerationResponseList>(chat.Content[0].Text, DefaultJsonOptions)!;

        if (questions == null || questions.Questions == null || questions.Questions.Count == 0)
            throw new InvalidOperationException(
                "AI returned no questions or invalid JSON");

        return questions.Questions;
    }

    public async Task<AIAnalysisResponse> GenerateAnalysisFromAIAsync(AIAnalysisRequest analysisRequest, string aiModel, CancellationToken cancellationToken)
    {
        List<ChatMessage> chatMessages = await GetChatMessages(analysisRequest, "AnalysisPrompt.txt");

        ChatClient chatClient = _aIClient.GetChatClient(aiModel);

        JSchemaGenerator generator = new();
        string jsonSchema = generator.Generate(typeof(AIAnalysisResponse)).ToString();

        ChatCompletionOptions completionOptions = new()
        {
            Temperature = 1f,
            MaxOutputTokenCount = 15000,
            TopP = 1,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(jsonSchemaFormatName: "AIAnalysisResponse",
                                                                       jsonSchema: BinaryData.FromString(jsonSchema),
                                                                       jsonSchemaIsStrict: false)
        };

        ChatCompletion chat = await chatClient.CompleteChatAsync(messages: chatMessages,
                                                                 options: completionOptions,
                                                                 cancellationToken: cancellationToken);

        RecordAsync(analysisRequest, aiModel, chat);

        AIAnalysisResponse response =
            JsonSerializer.Deserialize<AIAnalysisResponse>(chat.Content[0].Text)!;

        response.AIModelVersion = chat.Model;
        response.GeneratedAt = chat.CreatedAt.DateTime;

        return response;

    }

    public async Task<List<AIQuestionGenerationResponse>> ExtractQuestionsFromDocumentAsync(
    AIQuestionExtractionRequest extractionRequest,
    string aiModel,
    CancellationToken cancellationToken)
    {
        string documentText = await ExtractTextFromDocument(extractionRequest);

        List<ChatMessage> chatMessages = await GetChatMessagesForExtraction(extractionRequest, documentText);

        ChatClient chatClient = _aIClient.GetChatClient(aiModel);

        JSchemaGenerator schemaGenerator = new();
        string jsonSchema = schemaGenerator.Generate(typeof(AIQuestionGenerationResponseList)).ToString();

        int tokenLimit = extractionRequest.MaxQuestions > 0
            ? Math.Min(extractionRequest.MaxQuestions * 150 + 1000, 20000)
            : 20000;

        ChatCompletionOptions completionOptions = new()
        {
            Temperature = 0.7f,
            MaxOutputTokenCount = tokenLimit,
            TopP = 0.95f,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                jsonSchemaFormatName: "QuestionExtraction",
                jsonSchema: BinaryData.FromString(jsonSchema),
                jsonSchemaIsStrict: true)
        };

        ChatCompletion chat = await chatClient.CompleteChatAsync(
            messages: chatMessages,
            options: completionOptions,
            cancellationToken: cancellationToken);

        RecordAsync(extractionRequest, aiModel, chat);

        if (string.IsNullOrWhiteSpace(chat.Content[0].Text))
            throw new InvalidOperationException("AI returned empty response");

        AIQuestionGenerationResponseList questions = JsonSerializer.Deserialize<AIQuestionGenerationResponseList>(
            chat.Content[0].Text,
            DefaultJsonOptions)!;

        if (questions == null || questions.Questions == null || questions.Questions.Count == 0)
            throw new InvalidOperationException("AI could not extract any questions from the document");

        if (extractionRequest.MaxQuestions > 0 && questions.Questions.Count > extractionRequest.MaxQuestions)
            return [.. questions.Questions.Take(extractionRequest.MaxQuestions)];

        return questions.Questions;
    }

    private static async Task<string> ExtractTextFromDocument(AIQuestionExtractionRequest request)
    {
        byte[] documentBytes = Convert.FromBase64String(request.DocumentContent);

        return request.MimeType switch
        {
            "application/pdf" => await ExtractTextFromPdf(documentBytes),
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => await ExtractTextFromDocx(documentBytes),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" or "application/vnd.ms-excel" => await ExtractTextFromExcel(documentBytes),
            "text/plain" => System.Text.Encoding.UTF8.GetString(documentBytes),
            _ => throw new NotSupportedException($"Document type {request.MimeType} is not supported"),
        };
    }

    private static async Task<string> ExtractTextFromPdf(byte[] documentBytes)
    {
        using MemoryStream stream = new(documentBytes);
        using var document = UglyToad.PdfPig.PdfDocument.Open(stream);

        StringBuilder text = new();

        foreach (var page in document.GetPages())
        {
            text.AppendLine(page.Text);
            text.AppendLine();
        }

        return text.ToString();
    }

    private static async Task<string> ExtractTextFromDocx(byte[] documentBytes)
    {
        using MemoryStream stream = new(documentBytes);
        using WordprocessingDocument doc = WordprocessingDocument.Open(stream, false);

        if (doc.MainDocumentPart == null)
            return string.Empty;

        return doc.MainDocumentPart.Document.Body?.InnerText ?? string.Empty;
    }

    private static async Task<string> ExtractTextFromExcel(byte[] documentBytes)
    {
        using MemoryStream stream = new(documentBytes);
        using var workbook = new XLWorkbook(stream);

        StringBuilder content = new();

        foreach (var worksheet in workbook.Worksheets)
        {
            content.AppendLine($"=== Sheet: {worksheet.Name} ===");
            content.AppendLine();

            var usedRange = worksheet.RangeUsed();
            if (usedRange != null)
            {
                foreach (var row in usedRange.Rows())
                {
                    var rowValues = row.Cells().Select(c => c.GetValue<string>());
                    content.AppendLine(string.Join("\t", rowValues));
                }
            }

            content.AppendLine();
        }

        return content.ToString();
    }

    private static async Task<List<ChatMessage>> GetChatMessagesForExtraction(
        AIQuestionExtractionRequest extractionRequest,
        string documentText)
    {
        string basePrompt = await GetPromptFileContentAsync("ExtractionPrompt.txt");

        var requestData = new
        {
            extractionRequest.FileName,
            extractionRequest.LanguageCode,
            extractionRequest.LessonName,
            extractionRequest.DifficultyLevel,
            extractionRequest.ExpectedQuestionType,
            extractionRequest.LearningObjectives,
            extractionRequest.MaxQuestions,
            extractionRequest.ExtractionPrompt
        };

        string requestJson = JsonSerializer.Serialize(requestData, DefaultJsonOptions);

        string fullPrompt = $@"{basePrompt}Request Parameters:{requestJson}Document Content:{documentText}";

        return [new SystemChatMessage(fullPrompt)];
    }

    private static void RecordAsync(object objectOfRequest, string aiModel, ChatCompletion chat)
    {
        _ = Task.Run(async () =>
        {
            string storageJson = JsonSerializer.Serialize(chat, DefaultJsonOptions);

            await RequestRecorder.RecordAsync(request: JsonSerializer.Serialize(objectOfRequest),
                                              response: storageJson,
                                              modelName: aiModel,
                                              cancellationToken: CancellationToken.None);
        }, CancellationToken.None);
    }

    private static async Task<string> GetPromptFileContentAsync(string promptFile)
    {
        string promptPath = Path.Combine(AppContext.BaseDirectory,
                                         "Services",
                                         "AIService",
                                         "Resources",
                                         promptFile);

        if (!File.Exists(promptPath))
            throw new FileNotFoundException($"AI prompt template not found at: {promptPath}", promptPath);
        return await File.ReadAllTextAsync(promptPath);
    }

    private static async Task<List<ChatMessage>> GetChatMessages(object generationRequest, string fileName)
    {
        string promptData = JsonSerializer.Serialize(generationRequest, DefaultJsonOptions);
        string basePrompt = await GetPromptFileContentAsync(fileName);
        return [new SystemChatMessage($"{basePrompt}\n{promptData}")];
    }
}

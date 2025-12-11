using Application.Services.AIService;
using Application.Services.AIService.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using OpenAI;
using OpenAI.Chat;
using System.Text.Json;

namespace Infrastructure.Adapters.AIService;

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

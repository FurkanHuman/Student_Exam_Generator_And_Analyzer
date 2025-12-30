using Application.Services.AIService;
using Application.Services.AIService.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema.Generation;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Adapters.AIService.Google;

internal class GoogleServiceAdapter : IAIService
{
    private readonly Client _client;
    private static readonly JsonSerializerOptions DefaultJsonOptions = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };

    public GoogleServiceAdapter(IConfiguration configuration)
    {
        _client = new(apiKey: configuration.GetSection("GoogleAIApiKey").Get<string>());
    }

    public async Task<List<AIQuestionGenerationResponse>> GenerateQuestionsFromAIAsync(
        AIQuestionGenerationRequest generationRequest,
        string aiModel,
        CancellationToken cancellationToken)
    {
        string promptContent = await GetPromptContentAsync(generationRequest, "QuestionPrompt.txt");

        JSchemaGenerator schemaGenerator = new();
        string rawSchema = schemaGenerator.Generate(typeof(AIQuestionGenerationResponseList)).ToString();
        string cleanedSchema = CleanSchema(rawSchema);
        Schema? jsonSchemaGoogle = Schema.FromJson(cleanedSchema, DefaultJsonOptions);

        GenerateContentConfig generationConfig = new()
        {
            Temperature = 1f,
            MaxOutputTokens = 20000,
            TopP = 1f,
            ResponseMimeType = "application/json",
            ResponseSchema = jsonSchemaGoogle
        };

        GenerateContentResponse response = await _client.Models.GenerateContentAsync(
            model: aiModel,
            contents: promptContent,
            config: generationConfig
        );

        RecordAsync(generationRequest, aiModel, response);

        string responseText = response.Candidates?[0]?.Content?.Parts?[0]?.Text
            ?? throw new InvalidOperationException("AI returned empty or invalid response structure");

        AIQuestionGenerationResponseList questions = JsonSerializer.Deserialize<AIQuestionGenerationResponseList>(
            responseText,
            DefaultJsonOptions
        ) ?? throw new InvalidOperationException("Failed to deserialize AI response");

        if (questions.Questions == null || questions.Questions.Count == 0)
            throw new InvalidOperationException("AI returned no questions");

        return questions.Questions;
    }

    public async Task<AIAnalysisResponse> GenerateAnalysisFromAIAsync(
        AIAnalysisRequest analysisRequest,
        string aiModel,
        CancellationToken cancellationToken)
    {
        string promptContent = await GetPromptContentAsync(analysisRequest, "AnalysisPrompt.txt");

        JSchemaGenerator schemaGenerator = new();
        string rawSchema = schemaGenerator.Generate(typeof(AIAnalysisResponse)).ToString();
        string cleanedSchema = CleanSchema(rawSchema);
        Schema? jsonSchemaGoogle = Schema.FromJson(cleanedSchema, DefaultJsonOptions);

        GenerateContentConfig generationConfig = new()
        {
            Temperature = 1f,
            MaxOutputTokens = 15000,
            TopP = 1f,
            ResponseMimeType = "application/json",
            ResponseSchema = jsonSchemaGoogle
        };

        GenerateContentResponse response = await _client.Models.GenerateContentAsync(
            model: aiModel,
            contents: promptContent,
            config: generationConfig
        );

        RecordAsync(analysisRequest, aiModel, response);

        string responseText = response.Candidates?[0]?.Content?.Parts?[0]?.Text
            ?? throw new InvalidOperationException("AI returned empty or invalid response structure");

        AIAnalysisResponse result = JsonSerializer.Deserialize<AIAnalysisResponse>(
            responseText,
            DefaultJsonOptions
        ) ?? throw new InvalidOperationException("Failed to deserialize AI response");

        result.AIModelVersion = aiModel;
        result.GeneratedAt = DateTime.UtcNow;

        return result;
    }

    public async Task<List<AIQuestionGenerationResponse>> ExtractQuestionsFromDocumentAsync(
        AIQuestionExtractionRequest extractionRequest,
        string aiModel,
        CancellationToken cancellationToken)
    {
        // Prepare the prompt
        string promptContent = await GetPromptContentForExtraction(extractionRequest);

        // Generate JSON schema
        JSchemaGenerator schemaGenerator = new();
        string rawSchema = schemaGenerator.Generate(typeof(AIQuestionGenerationResponseList)).ToString();
        string cleanedSchema = CleanSchema(rawSchema);
        Schema? jsonSchemaGoogle = Schema.FromJson(cleanedSchema, DefaultJsonOptions);

        // Prepare document part based on MIME type
        Part documentPart = await CreateDocumentPart(extractionRequest);

        // Create content with both document and text prompt
        Content content = new()
        {
            Role = "user",
            Parts =
            [
                documentPart,
                new Part { Text = promptContent }
            ]
        };

        // Configure generation
        GenerateContentConfig generationConfig = new()
        {
            Temperature = 0.7f, // Lower temperature for more consistent extraction
            MaxOutputTokens = 20000,
            TopP = 0.95f,
            ResponseMimeType = "application/json",
            ResponseSchema = jsonSchemaGoogle
        };

        // Generate response
        GenerateContentResponse response = await _client.Models.GenerateContentAsync(
            model: aiModel,
            contents: content,
            config: generationConfig
        );

        RecordAsync(extractionRequest, aiModel, response);

        // Parse response
        string responseText = response.Candidates?[0]?.Content?.Parts?[0]?.Text
            ?? throw new InvalidOperationException("AI returned empty or invalid response structure");

        AIQuestionGenerationResponseList questions = JsonSerializer.Deserialize<AIQuestionGenerationResponseList>(
            responseText,
            DefaultJsonOptions
        ) ?? throw new InvalidOperationException("Failed to deserialize AI response");

        if (questions.Questions == null || questions.Questions.Count == 0)
            throw new InvalidOperationException("AI could not extract any questions from the document");

        // Apply max questions limit if specified
        if (extractionRequest.MaxQuestions > 0 && questions.Questions.Count > extractionRequest.MaxQuestions)
            return [.. questions.Questions.Take(extractionRequest.MaxQuestions)];

        return questions.Questions;
    }

    private static async Task<Part> CreateDocumentPart(AIQuestionExtractionRequest request)
    {
        byte[] documentBytes = Convert.FromBase64String(request.DocumentContent);

        // For PDFs, Google AI can process them directly
        if (request.MimeType == "application/pdf")
        {
            return new Part
            {
                InlineData = new Blob
                {
                    MimeType = request.MimeType,
                    Data = documentBytes
                }
            };
        }

        // For Word documents (DOCX)
        if (request.MimeType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
        {
            string extractedText = await ExtractTextFromDocx(documentBytes);
            return new Part { Text = $"Document Content ({request.FileName}):\n\n{extractedText}" };
        }

        // For Excel files
        if (request.MimeType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ||
            request.MimeType == "application/vnd.ms-excel")
        {
            string extractedText = await ExtractTextFromExcel(documentBytes);
            return new Part { Text = $"Spreadsheet Content ({request.FileName}):\n\n{extractedText}" };
        }

        // For plain text files
        if (request.MimeType == "text/plain")
        {
            string textContent = System.Text.Encoding.UTF8.GetString(documentBytes);
            return new Part { Text = $"Document Content ({request.FileName}):\n\n{textContent}" };
        }

        throw new NotSupportedException($"Document type {request.MimeType} is not supported");
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
            content.AppendLine($"Sheet: {worksheet.Name}");
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

    private static async Task<string> GetPromptContentForExtraction(AIQuestionExtractionRequest request)
    {
        string basePrompt = await GetPromptFileContentAsync("ExtractionPrompt.txt");

        var requestData = new
        {
            request.FileName,
            request.LanguageCode,
            request.LessonName,
            request.DifficultyLevel,
            request.ExpectedQuestionType,
            request.LearningObjectives,
            request.MaxQuestions,
            request.ExtractionPrompt,
            DocumentInfo = $"Document Type: {request.MimeType}, Size: {request.DocumentContent.Length} characters"
        };

        string requestJson = JsonSerializer.Serialize(requestData, DefaultJsonOptions);
        return $"{basePrompt}\n\nRequest Parameters:\n{requestJson}";
    }

    /// <summary>
    /// Recursively removes "additionalProperties" from the JSON schema string.
    /// Gemini API throws an error if this field is present.
    /// </summary>
    private static string CleanSchema(string jsonSchema)
    {
        JObject schemaObj = JObject.Parse(jsonSchema);
        RemoveAdditionalProperties(schemaObj);
        return schemaObj.ToString();
    }

    private static void RemoveAdditionalProperties(JToken token)
    {
        if (token is JObject obj)
        {
            obj.Remove("additionalProperties");

            foreach (JProperty property in obj.Properties())
                RemoveAdditionalProperties(property.Value);
        }

        else if (token is JArray array)
            foreach (JToken item in array)
                RemoveAdditionalProperties(item);

    }

    private static void RecordAsync(object objectOfRequest, string aiModel, GenerateContentResponse response)
    {
        _ = Task.Run(async () =>
        {
            string storageJson = JsonSerializer.Serialize(response, DefaultJsonOptions);

            await RequestRecorder.RecordAsync(
                request: JsonSerializer.Serialize(objectOfRequest),
                response: storageJson,
                modelName: aiModel,
                cancellationToken: CancellationToken.None
            );
        }, CancellationToken.None);
    }

    private static async Task<string> GetPromptFileContentAsync(string promptFile)
    {
        string promptPath = Path.Combine(
            AppContext.BaseDirectory,
            "Services",
            "AIService",
            "Resources",
            promptFile
        );

        if (!System.IO.File.Exists(promptPath))
            throw new FileNotFoundException($"AI prompt template not found at: {promptPath}", promptPath);

        return await System.IO.File.ReadAllTextAsync(promptPath);
    }

    private static async Task<string> GetPromptContentAsync(object request, string fileName)
    {
        string promptData = JsonSerializer.Serialize(request, DefaultJsonOptions);
        string basePrompt = await GetPromptFileContentAsync(fileName);
        return $"{basePrompt}\n{promptData}";
    }
}
using Application.Services.AIService;
using Application.Services.AIService.Models;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Linq;
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
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
        WriteIndented = false
    };

    public OpenAIServiceAdapter(IConfiguration configuration)
    {

        _aIClient = new(configuration.GetSection("OpenAiApiKey").Get<string>());
    }

    public async Task<ICollection<QuestionAIInComingModel>?> GenerateQuestionsFromAIAsync(QuestionAIOutgoingModel outgoingModel, string aiModel, CancellationToken cancellationToken)
    {
        string promptPath = Path.Combine(AppContext.BaseDirectory, "Application", "Services", "AIService", "Resources", "QuestionSystemPromptNew");

        string basePrompt = await File.ReadAllTextAsync(promptPath, cancellationToken);


        string outgoingModeljJsonStr = JsonSerializer.Serialize(outgoingModel);
        List<ChatMessage> chatMessages =
            [
                new SystemChatMessage($"{basePrompt}\n{outgoingModeljJsonStr}")
            ];

        ChatClient chatClient = _aIClient.GetChatClient(aiModel);

        JSchemaGenerator generator = new();                                                       // .NET 9.0 and above, use the Microsoft "System.Text.Json;" library
        string jsonSchema = generator.Generate(typeof(List<QuestionAIInComingModel>)).ToString(); // use "System.Text.Json.Schema", "System.Text.Json" library


        ChatCompletionOptions completionOptions = new()
        {
            Temperature = 1f,
            MaxOutputTokenCount = 1500,
            TopP = 1,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(jsonSchemaFormatName: "QuizQuestion", jsonSchema: BinaryData.FromString(jsonSchema), jsonSchemaIsStrict: true)
        };

        List<QuestionAIInComingModel>? questions;

        ChatCompletion chatCompletion = await chatClient.CompleteChatAsync(messages: chatMessages, completionOptions, cancellationToken: cancellationToken);

        string response = chatCompletion.Content[0].Text;

        questions = JsonSerializer.Deserialize<List<QuestionAIInComingModel>>(response);

        await RequestRecorder.RecordAsync(request: outgoingModeljJsonStr, response: response, modelName: aiModel, cancellationToken: cancellationToken);

        return questions;
    }
    public async Task<AIAnalysisResponse> GenerateAnalysisFromAIAsync(AIAnalysisRequest analysisRequest, string provider, string aiModel, CancellationToken cancellationToken)
    {
        string promptData = JsonSerializer.Serialize(analysisRequest, DefaultJsonOptions);

        string promptPath = Path.Combine(AppContext.BaseDirectory, "Application", "Services", "AIService", "Resources", "analysis_prompt.txt");

        string basePrompt = await File.ReadAllTextAsync(promptPath, cancellationToken);

        List<ChatMessage> chatMessages =
        [
            new SystemChatMessage($"{basePrompt}\n{promptData}")
        ];

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

        _ = Task.Run(async () =>
        {
            string storageJson = JsonSerializer.Serialize(chat, DefaultJsonOptions);

            await RequestRecorder.RecordAsync(request: JsonSerializer.Serialize(analysisRequest),
                                              response: storageJson,
                                              modelName: aiModel,
                                              cancellationToken: cancellationToken);
        }, cancellationToken);

        AIAnalysisResponse response =
            JsonSerializer.Deserialize<AIAnalysisResponse>(chat.Content[0].Text)!;

        response.AIModelVersion = chat.Model;
        response.GeneratedAt = chat.CreatedAt.DateTime;

        return response;
    }

}

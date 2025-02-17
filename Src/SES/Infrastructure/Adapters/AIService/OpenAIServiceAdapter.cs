using Application.Services.AIService;
using Application.Services.AIService.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Schema.Generation;
using OpenAI;
using OpenAI.Chat;
using System.Text.Json;

namespace Infrastructure.Adapters.AIService;
// Note: TUR: şu anlık devre dışı
public class OpenAIServiceAdapter : IAIService
{
    private readonly OpenAIClient _aIClient;
    private readonly string _systemPrompt;

    private const string SYSTEMPromptPath = @"\Student_Exam_Generator_And_Analyzer\Src\SES\Application\Services\AIService\Resources\QuestionSystemPromptNew.txt";

    public OpenAIServiceAdapter(IConfiguration configuration)
    {
        _systemPrompt = File.ReadAllText(SYSTEMPromptPath);
        _aIClient = new(configuration.GetSection("OpenAiApiKey").Get<string>());
    }

    public async Task<ICollection<QuestionAIInComingModel>?> GenerateQuestionsFromAIAsync(QuestionAIOutgoingModel outgoingModel, string aiModel, CancellationToken cancellationToken)
    {
        string outgoingModeljJsonStr = JsonSerializer.Serialize(outgoingModel);
        List<ChatMessage> chatMessages =
            [
                new UserChatMessage(_systemPrompt +"\n"+ outgoingModeljJsonStr)
            ];

        ChatClient chatClient = _aIClient.GetChatClient(aiModel);

        JSchemaGenerator generator = new JSchemaGenerator();                                // .NET 9.0 and above, use the Microsoft "System.Text.Json;" library
        string jsonSchema = generator.Generate(typeof(List<QuestionAIInComingModel>)).ToString(); // use "System.Text.Json.Schema", "System.Text.Json" library


        ChatCompletionOptions completionOptions = new()
        {
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(jsonSchemaFormatName: "QuizQuestion", jsonSchema: BinaryData.FromString(jsonSchema), jsonSchemaIsStrict: true),
            Temperature = 1f,
            MaxOutputTokenCount = 1500,
            TopP = 1,
            FrequencyPenalty = 0,
            PresencePenalty = 0
        };

        List<QuestionAIInComingModel>? questions;

        ChatCompletion chatCompletion = await chatClient.CompleteChatAsync(messages: chatMessages, cancellationToken: cancellationToken);

        string response = chatCompletion.Content[0].Text;

        questions = JsonSerializer.Deserialize<List<QuestionAIInComingModel>>(response);

        await RequestRecorder.RecordAsync(request: _systemPrompt + "\n" + outgoingModeljJsonStr, response: response, modelName: aiModel, cancellationToken: cancellationToken);

        return questions;
    }
}

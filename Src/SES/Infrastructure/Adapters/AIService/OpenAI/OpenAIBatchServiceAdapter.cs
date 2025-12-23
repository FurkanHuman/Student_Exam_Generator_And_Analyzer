using Application.Services.AIService;
using Application.Services.AIService.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using OpenAI.Batch;
using OpenAI.Chat;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Adapters.AIService.OpenAI;

#pragma warning disable OPENAI001
internal class OpenAIBatchServiceAdapter : IAIBatchService
{
    private readonly BatchClient _batchClient;

    private static readonly JsonSerializerOptions DefaultJsonOptions = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };

    public OpenAIBatchServiceAdapter(IConfiguration configuration)
    {
        _batchClient = new(configuration.GetSection("OpenAiApiKey").Get<string>());
    }

    public Task<List<AIAnalysisResponse>> GenerateBatchAnalysisAsync(List<AIAnalysisRequest> requests, string aiModel, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    //public async Task<List<AIAnalysisResponse>> GenerateBatchAnalysisAsync(List<AIAnalysisRequest> requests, string aiModel, CancellationToken cancellationToken)
    //{
    //    1.Create JSONL file content for batch requests

    //   string jsonlContent = await CreateBatchRequestsJsonl(requests, aiModel);

    //    2.Upload the JSONL file

    //   byte[] jsonlBytes = Encoding.UTF8.GetBytes(jsonlContent);
    //    using MemoryStream stream = new(jsonlBytes);

    //    var uploadedFile = await _batchClient.UploadBatchInputFileAsync(
    //        stream,
    //        "batch_requests.jsonl",
    //        cancellationToken
    //    );

    //    3.Create batch job
    //    var batchJob = await _batchClient.CreateBatchAsync(
    //        inputFileId: uploadedFile.Id,
    //        endpoint: "/v1/chat/completions",
    //        completionWindow: "24h",
    //        cancellationToken: cancellationToken
    //    );

    //    4.Wait for batch completion

    //   var completedBatch = await WaitForBatchCompletionAsync(batchJob.Id, cancellationToken);

    //    5.Download and parse results

    //   var results = await DownloadAndParseBatchResultsAsync(
    //       completedBatch.OutputFileId!,
    //       requests,
    //       cancellationToken7
    //   );

    //    return results;
    //}

    //private async Task<string> CreateBatchRequestsJsonl(List<AIAnalysisRequest> requests, string aiModel)
    //{
    //    StringBuilder jsonlBuilder = new();
    //    string basePrompt = await GetPromptFileContentAsync("AnalysisPrompt.txt");

    //    JSchemaGenerator generator = new();
    //    string jsonSchema = generator.Generate(typeof(AIAnalysisResponse)).ToString();

    //    for (int i = 0; i < requests.Count; i++)
    //    {
    //        AIAnalysisRequest request = requests[i];
    //        string promptData = JsonSerializer.Serialize(request, DefaultJsonOptions);
    //        string fullPrompt = $"{basePrompt}\n{promptData}";

    //        var batchRequest = new
    //        {
    //            custom_id = $"request-{i}",
    //            method = "POST",
    //            url = "/v1/chat/completions",
    //            body = new
    //            {
    //                model = aiModel,
    //                messages = new[]
    //                {
    //                    new { role = "system", content = fullPrompt }
    //                },
    //                temperature = 1f,
    //                max_tokens = 15000,
    //                top_p = 1,
    //                frequency_penalty = 0,
    //                presence_penalty = 0,
    //                response_format = new
    //                {
    //                    type = "json_schema",
    //                    json_schema = new
    //                    {
    //                        name = "AIAnalysisResponse",
    //                        schema = JsonSerializer.Deserialize<object>(jsonSchema),
    //                        strict = false
    //                    }
    //                }
    //            }
    //        };

    //        string jsonLine = JsonSerializer.Serialize(batchRequest, DefaultJsonOptions);
    //        jsonlBuilder.AppendLine(jsonLine);
    //    }

    //    return jsonlBuilder.ToString();
    //}

    //private async Task<BatchJob> WaitForBatchCompletionAsync(string batchId, CancellationToken cancellationToken)
    //{
    //    while (!cancellationToken.IsCancellationRequested)
    //    {
    //        var batch = await _batchClient.GetBatchAsync(batchId, cancellationToken);

    //        if (batch.Status == BatchJobStatus.Completed)
    //            return batch;

    //        if (batch.Status == BatchJobStatus.Failed ||
    //            batch.Status == BatchJobStatus.Expired ||
    //            batch.Status == BatchJobStatus.Cancelled)
    //        {
    //            throw new InvalidOperationException($"Batch job failed with status: {batch.Status}");
    //        }

    //        Wait before checking again
    //        await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
    //    }

    //    throw new OperationCanceledException("Batch job was cancelled");
    //}

    //private async Task<List<AIAnalysisResponse>> DownloadAndParseBatchResultsAsync(
    //    string outputFileId,
    //    List<AIAnalysisRequest> originalRequests,
    //    CancellationToken cancellationToken)
    //{
    //    Download the output file
    //    var outputFile = await _batchClient.DownloadBatchOutputFileAsync(
    //        outputFileId,
    //        cancellationToken
    //    );

    //    List<AIAnalysisResponse> results = [];

    //    using StreamReader reader = new(outputFile);
    //    string? line;

    //    while ((line = await reader.ReadLineAsync(cancellationToken)) != null)
    //    {
    //        if (string.IsNullOrWhiteSpace(line))
    //            continue;

    //        var batchResponse = JsonSerializer.Deserialize<BatchResponseItem>(line, DefaultJsonOptions);

    //        if (batchResponse?.Response?.Body?.Choices != null &&
    //            batchResponse.Response.Body.Choices.Count > 0)
    //        {
    //            string content = batchResponse.Response.Body.Choices[0].Message.Content;

    //            var analysisResponse = JsonSerializer.Deserialize<AIAnalysisResponse>(
    //                content,
    //                DefaultJsonOptions
    //            )!;

    //            analysisResponse.AIModelVersion = batchResponse.Response.Body.Model;
    //            analysisResponse.GeneratedAt = DateTime.UtcNow;

    //            results.Add(analysisResponse);
    //        }
    //    }

    //    Record batch results
    //   _ = Task.Run(async () =>
    //   {
    //       await RequestRecorder.RecordAsync(
    //           request: JsonSerializer.Serialize(originalRequests),
    //           response: JsonSerializer.Serialize(results),
    //           modelName: "batch-operation",
    //           cancellationToken: CancellationToken.None
    //       );
    //   }, CancellationToken.None);

    //    return results;
    //}

    //private static async Task<string> GetPromptFileContentAsync(string promptFile)
    //{
    //    string promptPath = Path.Combine(
    //        AppContext.BaseDirectory,
    //        "Services",
    //        "AIService",
    //        "Resources",
    //        promptFile
    //    );

    //    if (!File.Exists(promptPath))
    //        throw new FileNotFoundException($"AI prompt template not found at: {promptPath}", promptPath);

    //    return await File.ReadAllTextAsync(promptPath);
    //}

    //Helper classes for deserialization
    //private class BatchResponseItem
    //{
    //    public string? CustomId { get; set; }
    //    public BatchResponse? Response { get; set; }
    //}

    //private class BatchResponse
    //{
    //    public int StatusCode { get; set; }
    //    public BatchResponseBody? Body { get; set; }
    //}

    //private class BatchResponseBody
    //{
    //    public string? Model { get; set; }
    //    public List<BatchChoice>? Choices { get; set; }
    //}

    //private class BatchChoice
    //{
    //    public BatchMessage? Message { get; set; }
    //}

    //private class BatchMessage
    //{
    //    public string Content { get; set; } = string.Empty;
    //}
}
#pragma warning restore OPENAI001
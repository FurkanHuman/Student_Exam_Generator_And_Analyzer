using Application.Services.AIService;
using Application.Services.AIService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;
using OpenAI.Batch;
using OpenAI.Files;
using System.ClientModel;
using System.ClientModel.Primitives;
using System.Text;

namespace Infrastructure.Adapters.AIService.OpenAI;

#pragma warning disable OPENAI001
internal class OpenAIBatchServiceAdapter : IAIBatchService
{
    private readonly BatchClient _batchClient;
    private readonly OpenAIFileClient _fileClient;
    private readonly ILogger<OpenAIBatchServiceAdapter> _logger;

    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.None,
        ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
    };

    public OpenAIBatchServiceAdapter(IConfiguration configuration, ILogger<OpenAIBatchServiceAdapter> logger)
    {
        string _aiApiKey = configuration.GetSection("OpenAiApiKey").Get<string>()
             ?? throw new InvalidOperationException("OpenAI API Key not configured");

        _batchClient = new BatchClient(_aiApiKey);
        _fileClient = new OpenAIFileClient(_aiApiKey);
        _logger = logger;
    }

    public async Task<List<AIAnalysisResponse>> GenerateBatchAnalysisAsync(List<AIAnalysisRequest> aIAnalysisRequests, string model, CancellationToken cancellationToken)
    {

        _logger.LogWarning("Disabled Service: OpenAIBatchServiceAdapter ");
        return [];

        if (aIAnalysisRequests == null || aIAnalysisRequests.Count == 0)
        {
            _logger.LogWarning("Empty request list provided");
            return [];
        }

        _logger.LogInformation("Starting batch processing for {Count} requests", aIAnalysisRequests.Count);

        try
        {
            // 1. Create JSONL content
            string jsonlContent = await CreateBatchRequestsJsonlAsync(aIAnalysisRequests, model);

            // 2. Upload file
            OpenAIFile uploadedFile = await UploadBatchFileAsync(jsonlContent, cancellationToken);

            // 3. Create batch job
            OpenAIBatch batchJob = await CreateBatchJobAsync(uploadedFile.Id, cancellationToken);

            // 4. Wait for completion
            OpenAIBatch completedBatch = await WaitForBatchCompletionAsync(batchJob.Id, cancellationToken);

            // High-level business log (tek Information)
            _logger.LogInformation(
                "OpenAI batch lifecycle completed successfully. FileId: {FileId}, BatchId: {BatchId}",
                uploadedFile.Id,
                completedBatch.Id
            );

            // Technical details (Debug)
            _logger.LogDebug("Batch file uploaded. FileId: {FileId}", uploadedFile.Id);
            _logger.LogDebug("Batch job created. BatchId: {BatchId}", batchJob.Id);
            _logger.LogDebug("Batch job completed. BatchId: {BatchId}", completedBatch.Id);


            // 5. Download and parse results
            if (string.IsNullOrEmpty(completedBatch.OutputFileId))
            {
                _logger.LogError("Batch completed but output file ID is missing");
                return [];
            }

            List<AIAnalysisResponse> results = await DownloadAndParseBatchResultsAsync(
                completedBatch.OutputFileId,
                aIAnalysisRequests,
                model,
                cancellationToken
            );

            _logger.LogInformation("Batch processing completed. Retrieved {Count} results", results.Count);
            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during batch processing");
        }

        return null!;
    }

    private static async Task<string> CreateBatchRequestsJsonlAsync(List<AIAnalysisRequest> requests, string aiModel)
    {
        var jsonlBuilder = new StringBuilder();
        string basePrompt = await GetPromptFileContentAsync("AnalysisPrompt.txt");

        // Generate JSON schema for response format
        var generator = new JSchemaGenerator();
        var schema = generator.Generate(typeof(AIAnalysisResponse));
        var schemaDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(schema.ToString());

        foreach (var request in requests)
        {
            string requestData = JsonConvert.SerializeObject(request, JsonSettings);
            string fullPrompt = $"{basePrompt}\n\nAnalysis Data:\n{requestData}";

            var batchRequest = new
            {
                custom_id = $"analysis-{request.AnalysisId}",
                method = "POST",
                url = "/v1/chat/completions",
                body = new
                {
                    model = aiModel,
                    messages = new object[]
                    {
                        new { role = "system", content = "You are an expert educational data analyst. Analyze exam results and provide comprehensive insights." },
                        new { role = "user", content = fullPrompt }
                    },
                    temperature = 0.7,
                    max_tokens = 16000,
                    response_format = new
                    {
                        type = "json_schema",
                        json_schema = new
                        {
                            name = "AIAnalysisResponse",
                            schema = schemaDict,
                            strict = false
                        }
                    }
                }
            };

            string jsonLine = JsonConvert.SerializeObject(batchRequest, JsonSettings);
            jsonlBuilder.AppendLine(jsonLine);
        }

        return jsonlBuilder.ToString();
    }

    private async Task<OpenAIFile> UploadBatchFileAsync(string jsonlContent, CancellationToken cancellationToken)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(jsonlContent);
        using MemoryStream stream = new(bytes);

        string fileName = $"batch_analysis_{DateTime.UtcNow:yyyyMMddHHmmss}.jsonl";

        ClientResult<OpenAIFile> uploadedFile = await _fileClient.UploadFileAsync(
            stream,
            fileName,
            FileUploadPurpose.Batch,
            cancellationToken
        );

        return uploadedFile.Value;
    }

    private async Task<OpenAIBatch> CreateBatchJobAsync(string fileId, CancellationToken cancellationToken)
    {
        // Use protocol method for batch creation
        var batchRequest = new
        {
            input_file_id = fileId,
            endpoint = "/v1/chat/completions",
            completion_window = "24h"
        };

        BinaryContent content = BinaryContent.Create(BinaryData.FromObjectAsJson(batchRequest));

        RequestOptions requestOptions = new()
        {
            CancellationToken = cancellationToken
        };

        CreateBatchOperation result = await _batchClient.CreateBatchAsync(
            content,
            waitUntilCompleted: false,
            requestOptions
        );

        // Parse response
        BinaryData responseData = result.GetRawResponse().Content;
        var batchResponse = JsonConvert.DeserializeObject<OpenAIBatch>(responseData.ToString());

        return batchResponse ?? throw new InvalidOperationException("Failed to create batch job");
    }

    private async Task<OpenAIBatch> WaitForBatchCompletionAsync(string batchId, CancellationToken cancellationToken)
    {
        const int checkIntervalSeconds = 30;
        const int maxWaitHours = 25;
        int maxChecks = (maxWaitHours * 3600) / checkIntervalSeconds;
        int checkCount = 0;

        while (!cancellationToken.IsCancellationRequested && checkCount < maxChecks)
        {
            RequestOptions requestOptions = new()
            {
                CancellationToken = cancellationToken
            };

            ClientResult result = await _batchClient.GetBatchAsync(batchId, requestOptions);
            BinaryData responseData = result.GetRawResponse().Content;
            OpenAIBatch batch = JsonConvert.DeserializeObject<OpenAIBatch>(responseData.ToString())
                ?? throw new InvalidOperationException($"Failed to retrieve batch {batchId}");

            _logger.LogInformation(
                "Batch {BatchId} status: {Status}, Progress: {Completed}/{Total}",
                batchId,
                batch.Status,
                batch.RequestCounts?.Completed ?? 0,
                batch.RequestCounts?.Total ?? 0
            );

            if (batch.Status == "completed")
            {
                return batch;
            }

            if (batch.Status == "failed" || batch.Status == "expired" || batch.Status == "cancelled")
            {
                string errorMessage = batch.Errors?.FirstOrDefault()?.Message ?? "No error details available";
                throw new InvalidOperationException(
                    $"Batch job {batchId} failed with status: {batch.Status}. Error: {errorMessage}"
                );
            }

            await Task.Delay(TimeSpan.FromSeconds(checkIntervalSeconds), cancellationToken);
            checkCount++;
        }

        if (cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException($"Batch job {batchId} was cancelled by user");
        }

        throw new TimeoutException($"Batch job {batchId} did not complete within {maxWaitHours} hours");
    }

    private async Task<List<AIAnalysisResponse>> DownloadAndParseBatchResultsAsync(
       string outputFileId,
       List<AIAnalysisRequest> originalRequests,
       string aiModel,
       CancellationToken cancellationToken)
    {
        List<AIAnalysisResponse> results = [];
        Dictionary<string, AIAnalysisRequest> requestMap = originalRequests.ToDictionary(r => $"analysis-{r.AnalysisId}");

        try
        {
            BinaryData fileContent = await DownloadBatchFileAsync(outputFileId, cancellationToken);
            await ParseBatchFileAsync(fileContent, requestMap, aiModel, results, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading/parsing batch results");
        }

        FireAndForgetRecord(originalRequests, results, aiModel);
        return results;
    }

    private async Task<BinaryData> DownloadBatchFileAsync(
    string outputFileId,
    CancellationToken cancellationToken)
    {
        ClientResult result = await _fileClient.DownloadFileAsync(outputFileId, cancellationToken);
        return result.GetRawResponse().Content;
    }

    private async Task ParseBatchFileAsync(
    BinaryData fileContent,
    Dictionary<string, AIAnalysisRequest> requestMap,
    string aiModel,
    List<AIAnalysisResponse> results,
    CancellationToken cancellationToken)
    {
        using StringReader reader = new(fileContent.ToString());
        string? line;

        while ((line = await reader.ReadLineAsync(cancellationToken)) != null)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            ProcessBatchLine(line, requestMap, aiModel, results);
        }
    }

    private void ProcessBatchLine(
    string line,
    Dictionary<string, AIAnalysisRequest> requestMap,
    string aiModel,
    List<AIAnalysisResponse> results)
    {
        try
        {
            BatchResponseItem? batchResponse =
                JsonConvert.DeserializeObject<BatchResponseItem>(line);

            if (batchResponse == null)
                return;

            if (TryParseSuccessResponse(batchResponse, requestMap, aiModel, out var response))
            {
                results.Add(response);
                return;
            }

            LogBatchError(batchResponse);
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Failed to parse batch response line");
        }
    }

    private static bool TryParseSuccessResponse(
    BatchResponseItem batchResponse,
    Dictionary<string, AIAnalysisRequest> requestMap,
    string aiModel,
    out AIAnalysisResponse response)
    {
        response = null!;

        BatchChoice[]? choices = batchResponse.Response?.Body?.Choices;
        if (choices == null || choices.Length == 0)
            return false;

        string? content = choices[0].Message?.Content;
        if (string.IsNullOrEmpty(content))
            return false;

        response = JsonConvert.DeserializeObject<AIAnalysisResponse>(content)!;
        if (response == null)
            return false;

        if (!string.IsNullOrEmpty(batchResponse.CustomId) && requestMap.TryGetValue(batchResponse.CustomId, out var original))
            response.AnalysisId = original.AnalysisId;


        response.AIModelVersion = batchResponse.Response!.Body!.Model ?? aiModel;
        response.GeneratedAt = DateTime.UtcNow;

        return true;
    }

    private void LogBatchError(BatchResponseItem batchResponse)
    {
        if (batchResponse.Error == null)
            return;

        _logger.LogWarning(
            "Error in batch response for {CustomId}: {ErrorMessage}",
            batchResponse.CustomId,
            batchResponse.Error.Message
        );
    }

    private void FireAndForgetRecord(
    List<AIAnalysisRequest> originalRequests,
    List<AIAnalysisResponse> results,
    string aiModel)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await RequestRecorder.RecordAsync(
                    request: JsonConvert.SerializeObject(originalRequests, JsonSettings),
                    response: JsonConvert.SerializeObject(results, JsonSettings),
                    modelName: $"openai-batch-{aiModel}",
                    cancellationToken: CancellationToken.None
                );
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to record batch results");
            }
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

        if (!File.Exists(promptPath))
            throw new FileNotFoundException($"Prompt file not found: {promptPath}");


        return await File.ReadAllTextAsync(promptPath);
    }

    // Response models for deserialization
    private sealed class OpenAIBatch
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("object")]
        public string Object { get; set; } = string.Empty;

        [JsonProperty("endpoint")]
        public string Endpoint { get; set; } = string.Empty;

        [JsonProperty("errors")]
        public BatchError[]? Errors { get; set; }

        [JsonProperty("input_file_id")]
        public string InputFileId { get; set; } = string.Empty;

        [JsonProperty("completion_window")]
        public string CompletionWindow { get; set; } = string.Empty;

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("output_file_id")]
        public string? OutputFileId { get; set; }

        [JsonProperty("error_file_id")]
        public string? ErrorFileId { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        [JsonProperty("in_progress_at")]
        public long? InProgressAt { get; set; }

        [JsonProperty("expires_at")]
        public long? ExpiresAt { get; set; }

        [JsonProperty("finalizing_at")]
        public long? FinalizingAt { get; set; }

        [JsonProperty("completed_at")]
        public long? CompletedAt { get; set; }

        [JsonProperty("failed_at")]
        public long? FailedAt { get; set; }

        [JsonProperty("expired_at")]
        public long? ExpiredAt { get; set; }

        [JsonProperty("cancelling_at")]
        public long? CancellingAt { get; set; }

        [JsonProperty("cancelled_at")]
        public long? CancelledAt { get; set; }

        [JsonProperty("request_counts")]
        public RequestCounts? RequestCounts { get; set; }
    }

    private sealed class RequestCounts
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("completed")]
        public int Completed { get; set; }

        [JsonProperty("failed")]
        public int Failed { get; set; }
    }

    private sealed class BatchResponseItem
    {
        [JsonProperty("custom_id")]
        public string? CustomId { get; set; }

        [JsonProperty("response")]
        public BatchResponse? Response { get; set; }

        [JsonProperty("error")]
        public BatchError? Error { get; set; }
    }

    private sealed class BatchResponse
    {
        [JsonProperty("status_code")]
        public int StatusCode { get; set; }

        [JsonProperty("body")]
        public BatchResponseBody? Body { get; set; }
    }

    private sealed class BatchResponseBody
    {
        [JsonProperty("model")]
        public string? Model { get; set; }

        [JsonProperty("choices")]
        public BatchChoice[]? Choices { get; set; }
    }

    private sealed class BatchChoice
    {
        [JsonProperty("message")]
        public BatchMessage? Message { get; set; }
    }

    private sealed class BatchMessage
    {
        [JsonProperty("content")]
        public string Content { get; set; } = string.Empty;
    }

    private sealed class BatchError
    {
        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }
    }
}
#pragma warning restore OPENAI001
using Application.Services.AIService;
using Application.Services.AIService.Models;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Adapters.AIService.Google;

internal class GoogleBatchServiceAdapter : IAIBatchService
{
    private readonly Client _client;
    private readonly ILogger<GoogleBatchServiceAdapter> _logger;

    private static readonly JsonSerializerOptions DefaultJsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };

    public GoogleBatchServiceAdapter(
        IConfiguration configuration,
        ILogger<GoogleBatchServiceAdapter> logger)
    {
        string apiKey = configuration.GetSection("GoogleAIApiKey").Get<string>()
            ?? throw new InvalidOperationException("Google AI API Key not configured");

        _client = new Client(apiKey: apiKey);
        _logger = logger;
    }

    public async Task<List<AIAnalysisResponse>> GenerateBatchAnalysisAsync(
        List<AIAnalysisRequest> aIAnalysisRequests,
        string model,
        CancellationToken cancellationToken)
    {
        _logger.LogWarning("Disabled Service: GoogleBatchServiceAdapter ");
        return [];

        if (aIAnalysisRequests == null || aIAnalysisRequests.Count == 0)
        {
            _logger.LogWarning("Empty request list provided");
            return [];
        }

        _logger.LogInformation("Starting Google Gemini batch processing for {Count} requests", aIAnalysisRequests.Count);

        try
        {
            List<InlinedRequest> inlineRequests = await CreateInlineRequestsAsync(aIAnalysisRequests);

            BatchJob batchJob = await _client.Batches.CreateAsync(
                model: model,
                src: new BatchJobSource
                {
                    InlinedRequests = inlineRequests
                },
                config: new CreateBatchJobConfig
                {
                    DisplayName = $"ai-analysis-{DateTime.UtcNow:yyyyMMddHHmmss}"
                }
            );

            if (batchJob == null)
            {
                _logger.LogError("Failed to create batch job");
                return [];
            }

            _logger.LogInformation("Batch job created: {JobName}", batchJob.Name);

            BatchJob? completedJob = await WaitForBatchCompletionAsync(batchJob.Name, cancellationToken);

            if (completedJob == null)
            {
                _logger.LogError("Batch job did not complete successfully");
                return [];
            }

            _logger.LogInformation("Batch job completed: {JobName}", completedJob.Name);

            List<AIAnalysisResponse> results = ParseBatchResults(completedJob);

            _logger.LogInformation("Batch processing completed. Retrieved {Count} results", results.Count);

            // Record results asynchronously
            RecordResultsAsync(aIAnalysisRequests, results, model);

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during Google Gemini batch processing");
            return [];
        }
    }

    private static async Task<List<InlinedRequest>> CreateInlineRequestsAsync(List<AIAnalysisRequest> requests)
    {
        string basePrompt = await GetPromptFileContentAsync("AnalysisPrompt.txt");
        List<InlinedRequest> inlineRequests = [];

        string googleSchemaJson = GoogleSchemaConverter.CreateGoogleNativeSchema<AIAnalysisResponse>();
        Schema? responseSchema = Schema.FromJson(googleSchemaJson, DefaultJsonOptions);

        foreach (AIAnalysisRequest request in requests)
        {
            string requestData = JsonSerializer.Serialize(request, DefaultJsonOptions);
            string fullPrompt = $"{basePrompt}\n\nAnalysis Data:\n{requestData}";

            InlinedRequest inlineRequest = new()
            {
                Contents =
                [
                    new Content
                    {
                        Parts = [new Part { Text = fullPrompt }],
                        Role = "SYSTEM"
                    }
                ],
                Metadata = new Dictionary<string, string>
                {
                    ["custom_id"] = $"analysis-{request.AnalysisId}"
                },
                Config = new GenerateContentConfig
                {
                    ResponseMimeType = "application/json",
                    ResponseSchema = responseSchema
                }
            };

            inlineRequests.Add(inlineRequest);
        }

        return inlineRequests;
    }

    private async Task<BatchJob?> WaitForBatchCompletionAsync(string batchName, CancellationToken cancellationToken)
    {
        // todo: control this variable after removing hardcoding
        const int checkIntervalSeconds = 60;
        const int maxWaitHours = 25;
        int maxChecks = (maxWaitHours * 3600) / checkIntervalSeconds;
        int checkCount = 0;

        while (!cancellationToken.IsCancellationRequested && checkCount < maxChecks)
        {
            try
            {
                var batchJob = await _client.Batches.GetAsync(batchName);

                _logger.LogInformation(
                    "Batch job {JobName} state: {State}",
                    batchName,
                    batchJob.State
                );

                if (batchJob.State == JobState.JOB_STATE_SUCCEEDED)
                    return batchJob;


                if (batchJob.State == JobState.JOB_STATE_FAILED || batchJob.State == JobState.JOB_STATE_CANCELLED)
                {
                    _logger.LogError("Batch job {JobName} failed with state: {State}", batchName, batchJob.State);
                    return null;
                }

                await Task.Delay(TimeSpan.FromSeconds(checkIntervalSeconds), cancellationToken);
                checkCount++;
            }

            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error checking batch status for {JobName}, attempt {Attempt}", batchName, checkCount);

                if (checkCount >= maxChecks - 1)
                {
                    _logger.LogError("Failed to check batch status after {MaxChecks} attempts", maxChecks);
                    return null;
                }

                await Task.Delay(TimeSpan.FromSeconds(checkIntervalSeconds), cancellationToken);
                checkCount++;
            }
        }

        if (cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning("Batch job {JobName} was cancelled by user", batchName);
            return null;
        }

        _logger.LogError("Batch job {JobName} did not complete within {MaxWaitHours} hours", batchName, maxWaitHours);
        return null;
    }

    private List<AIAnalysisResponse> ParseBatchResults(BatchJob completedJob)
    {
        _logger.LogInformation("not implemented yet. parse method");  // todo: caon make this method. problem is dowland batches. how the batch dowland
        return [];
    }

    private void RecordResultsAsync(List<AIAnalysisRequest> requests, List<AIAnalysisResponse> results, string aiModel)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await RequestRecorder.RecordAsync(
                    request: JsonSerializer.Serialize(requests, DefaultJsonOptions),
                    response: JsonSerializer.Serialize(results, DefaultJsonOptions),
                    modelName: $"google-batch-{aiModel}",
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

        if (!System.IO.File.Exists(promptPath))
            _ = new FileNotFoundException($"Prompt file not found: {promptPath}");

        return await System.IO.File.ReadAllTextAsync(promptPath);
    }
}
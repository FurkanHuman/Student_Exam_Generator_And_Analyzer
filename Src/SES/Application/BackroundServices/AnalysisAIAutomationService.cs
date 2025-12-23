using Application.Services.AIAnalysis;
using Application.Services.AIService;
using Application.Services.AIService.Models;
using Application.Services.Analyses;
using Application.Services.CalculatorFactory;
using Application.Services.Exams;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NArchitecture.Core.Persistence.Paging;
using System.Text.Json;

namespace Application.BackroundServices;

internal class AnalysisAIAutomationService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<AnalysisAIAutomationService> _logger;
    private readonly IConfiguration _configuration;

    private IAnalysisService _analysisService;
    private AnalysisComputationEngine _analysisEngine;
    private IAIServiceFactory _aiServiceFactory;

    public AnalysisAIAutomationService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<AnalysisAIAutomationService> logger,
        IConfiguration configuration)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        bool enabled = _configuration.GetValue<bool>("BackgroundServices:AIAnalysisAutomation:Enabled");

        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        if (!enabled)
        {
            _logger.LogInformation("AI analysis automation service is disabled");
            return;
        }

        _analysisService = scope.ServiceProvider.GetRequiredService<IAnalysisService>();
        _analysisEngine = scope.ServiceProvider.GetRequiredService<AnalysisComputationEngine>();
        _aiServiceFactory = scope.ServiceProvider.GetRequiredService<IAIServiceFactory>();

        _logger.LogInformation("AI analysis automation service started\n\tBatch processes may take up to 24 hours.");

        while (!stoppingToken.IsCancellationRequested)
        {
            int intervalMinutes = _configuration.GetValue<int>("BackgroundServices:AIAnalysisAutomation:IntervalMinutes");

            await ProcessBatchAIAnalysis(stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(intervalMinutes), stoppingToken);
        }

        _logger.LogInformation("AI analysis automation service stopped");
    }

    private async Task ProcessBatchAIAnalysis(CancellationToken stoppingToken)
    {
        int batchSize = _configuration.GetValue<int>("BackgroundServices:AIAnalysisAutomation:BatchSize");

        IPaginate<Analysis>? analysesForAI = await _analysisService.GetListAsync(
            predicate: a => string.IsNullOrEmpty(a.AIResponse),
            include: a => a.Include(a => a.Lesson)
                          .Include(a => a.Semester)
                          .Include(a => a.Principal)
                          .Include(a => a.ReferenceBenefit)
                          .Include(a => a.StudentExamAnswers).ThenInclude(sea => sea.Student)
                          .Include(a => a.StudentExamAnswers).ThenInclude(sea => sea.StudentAnswers),
            index: 0,
            size: batchSize,
            cancellationToken: stoppingToken);

        if (analysesForAI == null || analysesForAI.Count == 0)
        {
            _logger.LogInformation("No analyses pending AI processing");
            return;
        }

        List<Analysis> analyses = [.. analysesForAI.Items];
        _logger.LogInformation("Processing batch of {Count} analyses", analyses.Count);

        List<AIAnalysisRequest> batchRequests = [];
        Dictionary<string, Analysis> analysisRequestMap = [];

        foreach (Analysis analysis in analyses)
        {
            if (stoppingToken.IsCancellationRequested)
                break;

            AIAnalysisRequest request = await PrepareAnalysisRequest(analysis, stoppingToken);
            batchRequests.Add(request);
            analysisRequestMap[analysis.Id.ToString()] = analysis;
        }

        if (batchRequests.Count == 0)
        {
            _logger.LogInformation("No valid requests prepared for batch processing");
            return;
        }

        string provider = AIModelsLoader.GetAIProviders().Keys.First();
        string model = AIModelsLoader.GetProviderModels()[provider].Keys.First();

        IAIBatchService batchService = _aiServiceFactory.GetBatchService(provider);

        List<AIAnalysisResponse> responses = await batchService.GenerateBatchAnalysisAsync(batchRequests, model, stoppingToken);

        _logger.LogInformation("Batch processing completed: {Count} responses received", responses.Count);

        await SaveBatchResults(analysisRequestMap, batchRequests, responses, stoppingToken);
    }

    private async Task<AIAnalysisRequest> PrepareAnalysisRequest(Analysis analysis, CancellationToken stoppingToken)
    {
        AnalysisGeneralExamStatisticsCounters generalStats =
            await _analysisEngine.GeneralStatsCalculator.CalculateAsync(analysis.Id, stoppingToken);

        IList<AnalysisDetailTableDto> detailTables =
            await _analysisEngine.DetailTableCalculator.CalculateAsync(analysis.Id, stoppingToken);

        Dictionary<int, double> questionAverages =
            await _analysisEngine.QuestionAveragesCalculator.CalculateAsync(analysis.Id, stoppingToken);

        SuccessDistribution successDistribution =
            await _analysisEngine.SuccessDistributionCalculator.CalculateAsync(analysis.Id, stoppingToken);

        AdvancedStatisticsData advancedStats =
            await _analysisEngine.AdvancedChartsCalculator.CalculateAsync(analysis.Id, stoppingToken);

        BenefitAnalysisResultDto benefitAnalysis =
            await _analysisEngine.BenefitAnalysisCalculator.CalculateAsync(analysis.Id, stoppingToken);

        return AnalysisAIRequestMapper.MapToAIRequest(
            analysisId: analysis.Id,
            analysis: analysis,
            generalStats: generalStats,
            detailTables: detailTables,
            questionAverages: questionAverages,
            successDistribution: successDistribution,
            benefitAnalysis: benefitAnalysis,
            advancedStats: advancedStats);
    }

    private async Task SaveBatchResults(Dictionary<string, Analysis> analysisRequestMap, List<AIAnalysisRequest> requests, List<AIAnalysisResponse> responses, CancellationToken stoppingToken)
    {
        int savedCount = 0;

        for (int i = 0; i < Math.Min(requests.Count, responses.Count); i++)
        {
            if (stoppingToken.IsCancellationRequested)
                break;

            string analysisId = requests[i].AnalysisId;

            if (!analysisRequestMap.TryGetValue(analysisId, out Analysis? analysis))
            {
                _logger.LogWarning("Analysis with ID {AnalysisId} not found in map", analysisId);
                continue;
            }

            analysis.AIResponse = JsonSerializer.Serialize(responses[i]);
            await _analysisService.UpdateAsync(analysis);
            savedCount++;

            _logger.LogInformation("Successfully saved AI response for analysis {AnalysisId}", analysisId);
        }

        _logger.LogInformation("Saved {Count} batch results out of {Total}", savedCount, requests.Count);
    }
}
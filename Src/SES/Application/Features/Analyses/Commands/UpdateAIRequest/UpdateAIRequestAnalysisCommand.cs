using Application.Features.Analyses.Rules;
using Application.Services.AIAnalysis;
using Application.Services.AIService;
using Application.Services.AIService.Models;
using Application.Services.CalculatorFactory;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using System.Text.Json;

namespace Application.Features.Analyses.Commands.UpdateAIRequest;

public class UpdateAIRequestAnalysisCommand : IRequest
{
    public int Id { get; set; }
    public required string Provider { get; set; }
    public required string Model { get; set; }

    public class UpdateAIRequestAnalysisCommandHandler : IRequestHandler<UpdateAIRequestAnalysisCommand>
    {
        private readonly IAnalysisRepository _analysisRepository;
        private readonly AnalysisBusinessRules _analysisBusinessRules;
        private readonly AnalysisComputationEngine _analysisEngine;
        private readonly IAIServiceFactory _aiServiceFactory;

        public UpdateAIRequestAnalysisCommandHandler(IAnalysisRepository analysisRepository, AnalysisBusinessRules analysisBusinessRules, AnalysisComputationEngine analysisEngine, IAIServiceFactory aiServiceFactory)
        {
            _analysisRepository = analysisRepository;
            _analysisBusinessRules = analysisBusinessRules;
            _analysisEngine = analysisEngine;
            _aiServiceFactory = aiServiceFactory;
        }

        public async Task Handle(UpdateAIRequestAnalysisCommand request, CancellationToken cancellationToken)
        {
            await _analysisBusinessRules.AnalysisIdShouldExistWhenSelected(request.Id, cancellationToken);
            Analysis? analysis = await _analysisRepository.GetAsync(a => a.Id == request.Id, cancellationToken: cancellationToken);
            await _analysisBusinessRules.AnalysisShouldExistWhenSelected(analysis);

            if (!string.IsNullOrEmpty(analysis!.AIResponse))
                return;

            _ = Task.Run(async () =>
            {
                AnalysisGeneralExamStatisticsCounters ge = await _analysisEngine.GeneralStatsCalculator.CalculateAsync(request.Id, cancellationToken);
                IList<AnalysisDetailTableDto> de = await _analysisEngine.DetailTableCalculator.CalculateAsync(request.Id, cancellationToken);
                Dictionary<int, double> qu = await _analysisEngine.QuestionAveragesCalculator.CalculateAsync(request.Id, cancellationToken);
                SuccessDistribution su = await _analysisEngine.SuccessDistributionCalculator.CalculateAsync(request.Id, cancellationToken);
                AdvancedStatisticsData ad = await _analysisEngine.AdvancedChartsCalculator.CalculateAsync(request.Id, cancellationToken);
                BenefitAnalysisResultDto be = await _analysisEngine.BenefitAnalysisCalculator.CalculateAsync(request.Id, cancellationToken);

                AIAnalysisRequest requestModel = AnalysisAIRequestMapper.MapToAIRequest(analysisId: request.Id,
                                                                                        analysis: analysis,
                                                                                        generalStats: ge,
                                                                                        detailTables: de,
                                                                                        questionAverages: qu,
                                                                                        successDistribution: su,
                                                                                        benefitAnalysis: be,
                                                                                        advancedStats: ad);

                IAIService aIService = _aiServiceFactory.GetService(request.Provider);

                AIAnalysisResponse aiResponse = await aIService.GenerateAnalysisFromAIAsync(requestModel, request.Provider, request.Model, cancellationToken);

                string responseToJson = JsonSerializer.Serialize(aiResponse);

                analysis.AIResponse = responseToJson;

                await _analysisRepository.UpdateAsync(analysis, cancellationToken);

            }, cancellationToken);
        }
    }
}
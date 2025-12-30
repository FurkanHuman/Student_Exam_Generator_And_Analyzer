using Application.Features.Analyses.Rules;
using Application.Services.AIService.Models;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using System.Text.Json;

namespace Application.Features.Analyses.Queries.GetByIdAnalysisAIResponse;

public class GetByIdAnalysisAIResponseQuery : IRequest<AIAnalysisResponse>
{
    public int Id { get; set; }

    public class GetByIdAnalysisAIResponseQueryHandler : IRequestHandler<GetByIdAnalysisAIResponseQuery, AIAnalysisResponse>
    {
        private readonly IAnalysisRepository _analysisRepository;
        private readonly AnalysisBusinessRules _analysisBusinessRules;

        public GetByIdAnalysisAIResponseQueryHandler(IAnalysisRepository analysisRepository, AnalysisBusinessRules analysisBusinessRules)
        {
            _analysisRepository = analysisRepository;
            _analysisBusinessRules = analysisBusinessRules;
        }

        public async Task<AIAnalysisResponse> Handle(GetByIdAnalysisAIResponseQuery request, CancellationToken cancellationToken)
        {

            await _analysisBusinessRules.AnalysisIdShouldExistWhenSelected(request.Id, cancellationToken);
            Analysis? analysis = await _analysisRepository.GetAsync(
                                                            predicate: a => a.Id == request.Id
                                                            , cancellationToken: cancellationToken
                                                            );
            await _analysisBusinessRules.AnalysisShouldExistWhenSelected(analysis);
            if (string.IsNullOrEmpty(analysis!.AIResponse))
                return null!;


            AIAnalysisResponse aiResponse = JsonSerializer.Deserialize<AIAnalysisResponse>(analysis!.AIResponse)!;
            return aiResponse;
        }
    }
}

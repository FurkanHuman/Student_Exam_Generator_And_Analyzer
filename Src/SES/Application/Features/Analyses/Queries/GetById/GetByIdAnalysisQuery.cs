using Application.Features.Analyses.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.Analyses.Constants.AnalysesOperationClaims;

namespace Application.Features.Analyses.Queries.GetById;

public class GetByIdAnalysisQuery : IRequest<GetByIdAnalysisResponse>, ISecuredRequest
{
    public int Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdAnalysisQueryHandler : IRequestHandler<GetByIdAnalysisQuery, GetByIdAnalysisResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAnalysisRepository _analysisRepository;
        private readonly AnalysisBusinessRules _analysisBusinessRules;

        public GetByIdAnalysisQueryHandler(IMapper mapper, IAnalysisRepository analysisRepository, AnalysisBusinessRules analysisBusinessRules)
        {
            _mapper = mapper;
            _analysisRepository = analysisRepository;
            _analysisBusinessRules = analysisBusinessRules;
        }

        public async Task<GetByIdAnalysisResponse> Handle(GetByIdAnalysisQuery request, CancellationToken cancellationToken)
        {
            Analysis? analysis = await _analysisRepository.GetAsync(predicate: a => a.Id == request.Id, cancellationToken: cancellationToken);
            await _analysisBusinessRules.AnalysisShouldExistWhenSelected(analysis);

            GetByIdAnalysisResponse response = _mapper.Map<GetByIdAnalysisResponse>(analysis);
            return response;
        }
    }
}
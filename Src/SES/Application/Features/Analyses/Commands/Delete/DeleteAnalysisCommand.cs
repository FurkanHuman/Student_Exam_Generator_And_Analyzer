using Application.Features.Analyses.Constants;
using Application.Features.Analyses.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Analyses.Constants.AnalysesOperationClaims;

namespace Application.Features.Analyses.Commands.Delete;

public class DeleteAnalysisCommand : IRequest<DeletedAnalysisResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }

    public string[] Roles => [Admin, Write, AnalysesOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetAnalyses"];

    public class DeleteAnalysisCommandHandler : IRequestHandler<DeleteAnalysisCommand, DeletedAnalysisResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAnalysisRepository _analysisRepository;
        private readonly AnalysisBusinessRules _analysisBusinessRules;

        public DeleteAnalysisCommandHandler(IMapper mapper, IAnalysisRepository analysisRepository,
                                         AnalysisBusinessRules analysisBusinessRules)
        {
            _mapper = mapper;
            _analysisRepository = analysisRepository;
            _analysisBusinessRules = analysisBusinessRules;
        }

        public async Task<DeletedAnalysisResponse> Handle(DeleteAnalysisCommand request, CancellationToken cancellationToken)
        {
            Analysis? analysis = await _analysisRepository.GetAsync(predicate: a => a.Id == request.Id, cancellationToken: cancellationToken);
            await _analysisBusinessRules.AnalysisShouldExistWhenSelected(analysis);

            await _analysisRepository.DeleteAsync(analysis!);

            DeletedAnalysisResponse response = _mapper.Map<DeletedAnalysisResponse>(analysis);
            return response;
        }
    }
}
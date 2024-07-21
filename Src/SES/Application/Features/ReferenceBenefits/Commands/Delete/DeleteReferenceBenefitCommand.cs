using Application.Features.ReferenceBenefits.Constants;
using Application.Features.ReferenceBenefits.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.ReferenceBenefits.Constants.ReferenceBenefitsOperationClaims;

namespace Application.Features.ReferenceBenefits.Commands.Delete;

public class DeleteReferenceBenefitCommand : IRequest<DeletedReferenceBenefitResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }

    public string[] Roles => [Admin, Write, ReferenceBenefitsOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetReferenceBenefits"];

    public class DeleteReferenceBenefitCommandHandler : IRequestHandler<DeleteReferenceBenefitCommand, DeletedReferenceBenefitResponse>
    {
        private readonly IMapper _mapper;
        private readonly IReferenceBenefitRepository _referenceBenefitRepository;
        private readonly ReferenceBenefitBusinessRules _referenceBenefitBusinessRules;

        public DeleteReferenceBenefitCommandHandler(IMapper mapper, IReferenceBenefitRepository referenceBenefitRepository,
                                         ReferenceBenefitBusinessRules referenceBenefitBusinessRules)
        {
            _mapper = mapper;
            _referenceBenefitRepository = referenceBenefitRepository;
            _referenceBenefitBusinessRules = referenceBenefitBusinessRules;
        }

        public async Task<DeletedReferenceBenefitResponse> Handle(DeleteReferenceBenefitCommand request, CancellationToken cancellationToken)
        {
            ReferenceBenefit? referenceBenefit = await _referenceBenefitRepository.GetAsync(predicate: rb => rb.Id == request.Id, cancellationToken: cancellationToken);
            await _referenceBenefitBusinessRules.ReferenceBenefitShouldExistWhenSelected(referenceBenefit);

            await _referenceBenefitRepository.DeleteAsync(referenceBenefit!);

            DeletedReferenceBenefitResponse response = _mapper.Map<DeletedReferenceBenefitResponse>(referenceBenefit);
            return response;
        }
    }
}
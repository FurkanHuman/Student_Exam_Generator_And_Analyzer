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

namespace Application.Features.ReferenceBenefits.Commands.Update;

public class UpdateReferenceBenefitCommand : IRequest<UpdatedReferenceBenefitResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
    public required string ReferenceBenefitName { get; set; }
    public required int SemesterId { get; set; }
    public required int SchoolId { get; set; }
    public required int ExamId { get; set; }
    public required int LearningAreaId { get; set; }
    public required Semester Semester { get; set; }

    public string[] Roles => [Admin, Write, ReferenceBenefitsOperationClaims.Update];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetReferenceBenefits"];

    public class UpdateReferenceBenefitCommandHandler : IRequestHandler<UpdateReferenceBenefitCommand, UpdatedReferenceBenefitResponse>
    {
        private readonly IMapper _mapper;
        private readonly IReferenceBenefitRepository _referenceBenefitRepository;
        private readonly ReferenceBenefitBusinessRules _referenceBenefitBusinessRules;

        public UpdateReferenceBenefitCommandHandler(IMapper mapper, IReferenceBenefitRepository referenceBenefitRepository,
                                         ReferenceBenefitBusinessRules referenceBenefitBusinessRules)
        {
            _mapper = mapper;
            _referenceBenefitRepository = referenceBenefitRepository;
            _referenceBenefitBusinessRules = referenceBenefitBusinessRules;
        }

        public async Task<UpdatedReferenceBenefitResponse> Handle(UpdateReferenceBenefitCommand request, CancellationToken cancellationToken)
        {
            ReferenceBenefit? referenceBenefit = await _referenceBenefitRepository.GetAsync(predicate: rb => rb.Id == request.Id, cancellationToken: cancellationToken);
            await _referenceBenefitBusinessRules.ReferenceBenefitShouldExistWhenSelected(referenceBenefit);
            referenceBenefit = _mapper.Map(request, referenceBenefit);

            await _referenceBenefitRepository.UpdateAsync(referenceBenefit!);

            UpdatedReferenceBenefitResponse response = _mapper.Map<UpdatedReferenceBenefitResponse>(referenceBenefit);
            return response;
        }
    }
}
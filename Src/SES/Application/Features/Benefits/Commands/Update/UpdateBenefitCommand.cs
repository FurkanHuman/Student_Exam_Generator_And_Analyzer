using Application.Features.Benefits.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;

namespace Application.Features.Benefits.Commands.Update;

public class UpdateBenefitCommand : IRequest<UpdatedBenefitResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
    public required int SubLearningAreaId { get; set; }
    public required string ReferenceBenefitNumber { get; set; }
    public required string ReferenceBenefitComments { get; set; }



    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetBenefits"];

    public class UpdateBenefitCommandHandler : IRequestHandler<UpdateBenefitCommand, UpdatedBenefitResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBenefitRepository _benefitRepository;
        private readonly BenefitBusinessRules _benefitBusinessRules;

        public UpdateBenefitCommandHandler(IMapper mapper, IBenefitRepository benefitRepository,
                                         BenefitBusinessRules benefitBusinessRules)
        {
            _mapper = mapper;
            _benefitRepository = benefitRepository;
            _benefitBusinessRules = benefitBusinessRules;
        }

        public async Task<UpdatedBenefitResponse> Handle(UpdateBenefitCommand request, CancellationToken cancellationToken)
        {
            Benefit? benefit = await _benefitRepository.GetAsync(predicate: b => b.Id == request.Id, cancellationToken: cancellationToken);
            await _benefitBusinessRules.BenefitShouldExistWhenSelected(benefit);
            benefit = _mapper.Map(request, benefit);

            await _benefitRepository.UpdateAsync(benefit!);

            UpdatedBenefitResponse response = _mapper.Map<UpdatedBenefitResponse>(benefit);
            return response;
        }
    }
}
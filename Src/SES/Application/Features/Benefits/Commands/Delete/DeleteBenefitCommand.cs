using Application.Features.Benefits.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;

namespace Application.Features.Benefits.Commands.Delete;

public class DeleteBenefitCommand : IRequest<DeletedBenefitResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }



    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetBenefits"];

    public class DeleteBenefitCommandHandler : IRequestHandler<DeleteBenefitCommand, DeletedBenefitResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBenefitRepository _benefitRepository;
        private readonly BenefitBusinessRules _benefitBusinessRules;

        public DeleteBenefitCommandHandler(IMapper mapper, IBenefitRepository benefitRepository,
                                         BenefitBusinessRules benefitBusinessRules)
        {
            _mapper = mapper;
            _benefitRepository = benefitRepository;
            _benefitBusinessRules = benefitBusinessRules;
        }

        public async Task<DeletedBenefitResponse> Handle(DeleteBenefitCommand request, CancellationToken cancellationToken)
        {
            Benefit? benefit = await _benefitRepository.GetAsync(predicate: b => b.Id == request.Id, cancellationToken: cancellationToken);
            await _benefitBusinessRules.BenefitShouldExistWhenSelected(benefit);

            await _benefitRepository.DeleteAsync(benefit!);

            DeletedBenefitResponse response = _mapper.Map<DeletedBenefitResponse>(benefit);
            return response;
        }
    }
}
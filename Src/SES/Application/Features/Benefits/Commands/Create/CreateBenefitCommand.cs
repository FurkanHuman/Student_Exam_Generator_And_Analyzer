using Application.Features.Benefits.Constants;
using Application.Features.Benefits.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Benefits.Constants.BenefitsOperationClaims;

namespace Application.Features.Benefits.Commands.Create;

public class CreateBenefitCommand : IRequest<CreatedBenefitResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required string BenefitCode { get; set; }
    public required string Description { get; set; }


    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetBenefits"];

    public class CreateBenefitCommandHandler : IRequestHandler<CreateBenefitCommand, CreatedBenefitResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBenefitRepository _benefitRepository;
        private readonly BenefitBusinessRules _benefitBusinessRules;

        public CreateBenefitCommandHandler(IMapper mapper, IBenefitRepository benefitRepository,
                                         BenefitBusinessRules benefitBusinessRules)
        {
            _mapper = mapper;
            _benefitRepository = benefitRepository;
            _benefitBusinessRules = benefitBusinessRules;
        }

        public async Task<CreatedBenefitResponse> Handle(CreateBenefitCommand request, CancellationToken cancellationToken)
        {
            Benefit benefit = _mapper.Map<Benefit>(request);

            await _benefitRepository.AddAsync(benefit);

            CreatedBenefitResponse response = _mapper.Map<CreatedBenefitResponse>(benefit);
            return response;
        }
    }
}
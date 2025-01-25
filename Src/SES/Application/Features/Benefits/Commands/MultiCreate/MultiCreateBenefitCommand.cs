using Application.Features.Benefits.Commands.Create;
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
using NArchitecture.Core.Application.Responses;
using static Application.Features.Benefits.Constants.BenefitsOperationClaims;

namespace Application.Features.Benefits.Commands.MultiCreate;

public class MultiCreateBenefitCommand : IRequest<GetListResponse<MultiCreatedBenefitResponse>>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public ICollection<MultiBenefitCommandDto> MultiBenefits { get; set; }

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetBenefits"];

    public class MultiCreateBenefitCommandHandler : IRequestHandler<MultiCreateBenefitCommand, GetListResponse<MultiCreatedBenefitResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IBenefitRepository _benefitRepository;
        private readonly BenefitBusinessRules _benefitBusinessRules;

        public MultiCreateBenefitCommandHandler(IMapper mapper, IBenefitRepository benefitRepository, BenefitBusinessRules benefitBusinessRules)
        {
            _mapper = mapper;
            _benefitRepository = benefitRepository;
            _benefitBusinessRules = benefitBusinessRules;
        }

        public async Task<GetListResponse<MultiCreatedBenefitResponse>> Handle(MultiCreateBenefitCommand request, CancellationToken cancellationToken)
        {
            Benefit[] benefits = _mapper.Map<Benefit[]>(request.MultiBenefits);
           
            _benefitRepository.AddRange(benefits);

            GetListResponse<MultiCreatedBenefitResponse> response = _mapper.Map<GetListResponse<MultiCreatedBenefitResponse>>(benefits);
            return response;
        }
    }
}
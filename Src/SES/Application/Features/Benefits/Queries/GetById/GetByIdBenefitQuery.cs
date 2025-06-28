using Application.Features.Benefits.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Benefits.Queries.GetById;

public class GetByIdBenefitQuery : IRequest<GetByIdBenefitResponse>
{
    public int Id { get; set; }



    public class GetByIdBenefitQueryHandler : IRequestHandler<GetByIdBenefitQuery, GetByIdBenefitResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBenefitRepository _benefitRepository;
        private readonly BenefitBusinessRules _benefitBusinessRules;

        public GetByIdBenefitQueryHandler(IMapper mapper, IBenefitRepository benefitRepository, BenefitBusinessRules benefitBusinessRules)
        {
            _mapper = mapper;
            _benefitRepository = benefitRepository;
            _benefitBusinessRules = benefitBusinessRules;
        }

        public async Task<GetByIdBenefitResponse> Handle(GetByIdBenefitQuery request, CancellationToken cancellationToken)
        {
            Benefit? benefit = await _benefitRepository.GetAsync(predicate: b => b.Id == request.Id, cancellationToken: cancellationToken);
            await _benefitBusinessRules.BenefitShouldExistWhenSelected(benefit);

            GetByIdBenefitResponse response = _mapper.Map<GetByIdBenefitResponse>(benefit);
            return response;
        }
    }
}
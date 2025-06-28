using Application.Features.ReferenceBenefits.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.ReferenceBenefits.Queries.GetById;

public class GetByIdReferenceBenefitQuery : IRequest<GetByIdReferenceBenefitResponse>
{
    public int Id { get; set; }



    public class GetByIdReferenceBenefitQueryHandler : IRequestHandler<GetByIdReferenceBenefitQuery, GetByIdReferenceBenefitResponse>
    {
        private readonly IMapper _mapper;
        private readonly IReferenceBenefitRepository _referenceBenefitRepository;
        private readonly ReferenceBenefitBusinessRules _referenceBenefitBusinessRules;

        public GetByIdReferenceBenefitQueryHandler(IMapper mapper, IReferenceBenefitRepository referenceBenefitRepository, ReferenceBenefitBusinessRules referenceBenefitBusinessRules)
        {
            _mapper = mapper;
            _referenceBenefitRepository = referenceBenefitRepository;
            _referenceBenefitBusinessRules = referenceBenefitBusinessRules;
        }

        public async Task<GetByIdReferenceBenefitResponse> Handle(GetByIdReferenceBenefitQuery request, CancellationToken cancellationToken)
        {
            ReferenceBenefit? referenceBenefit = await _referenceBenefitRepository.GetAsync(predicate: rb => rb.Id == request.Id, cancellationToken: cancellationToken);
            await _referenceBenefitBusinessRules.ReferenceBenefitShouldExistWhenSelected(referenceBenefit);

            GetByIdReferenceBenefitResponse response = _mapper.Map<GetByIdReferenceBenefitResponse>(referenceBenefit);
            return response;
        }
    }
}
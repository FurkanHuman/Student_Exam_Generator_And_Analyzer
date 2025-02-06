using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.ReferenceBenefits.Queries.GetListByIdReferenceBenefitBenefit;

public class GetListByIdReferenceBenefitBenefitQuery : IRequest<GetListResponse<GetListByIdReferenceBenefitBenefitDto>>
{
    public int Id { get; set; }

    public class GetListByIdReferenceBenefitBenefitQueryHandler : IRequestHandler<GetListByIdReferenceBenefitBenefitQuery, GetListResponse<GetListByIdReferenceBenefitBenefitDto>>
    {
        private readonly IReferenceBenefitRepository _referenceBenefitRepository;
        private readonly IMapper _mapper;

        public GetListByIdReferenceBenefitBenefitQueryHandler(IReferenceBenefitRepository referenceBenefitRepository, IMapper mapper)
        {
            _referenceBenefitRepository = referenceBenefitRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListByIdReferenceBenefitBenefitDto>> Handle(GetListByIdReferenceBenefitBenefitQuery request, CancellationToken cancellationToken)
        {
            ReferenceBenefit? referenceBenefit = await _referenceBenefitRepository.GetAsync(predicate: rb => rb.Id == request.Id,
                                                                                            include: rb => rb.Include(rb => rb.LearningAreas).ThenInclude(la => la.SubLearningAreas).ThenInclude(sla => sla.Benefits),
                                                                                            cancellationToken: cancellationToken);

            IList<GetListByIdReferenceBenefitBenefitDto> benefitDtos = [.. _mapper.Map<List<GetListByIdReferenceBenefitBenefitDto>>(referenceBenefit?.LearningAreas
                .SelectMany(la => la.SubLearningAreas)
                .SelectMany(sla => sla.Benefits))];

            GetListResponse<GetListByIdReferenceBenefitBenefitDto> response = new GetListResponse<GetListByIdReferenceBenefitBenefitDto>();
            response.Items = benefitDtos;

            return response;
        }
    }
}
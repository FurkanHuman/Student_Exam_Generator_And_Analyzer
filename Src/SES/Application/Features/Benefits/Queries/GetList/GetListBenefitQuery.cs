using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Benefits.Constants.BenefitsOperationClaims;

namespace Application.Features.Benefits.Queries.GetList;

public class GetListBenefitQuery : IRequest<GetListResponse<GetListBenefitListItemDto>>, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListBenefits({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetBenefits";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListBenefitQueryHandler : IRequestHandler<GetListBenefitQuery, GetListResponse<GetListBenefitListItemDto>>
    {
        private readonly IBenefitRepository _benefitRepository;
        private readonly IMapper _mapper;

        public GetListBenefitQueryHandler(IBenefitRepository benefitRepository, IMapper mapper)
        {
            _benefitRepository = benefitRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListBenefitListItemDto>> Handle(GetListBenefitQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Benefit> benefits = await _benefitRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListBenefitListItemDto> response = _mapper.Map<GetListResponse<GetListBenefitListItemDto>>(benefits);
            return response;
        }
    }
}
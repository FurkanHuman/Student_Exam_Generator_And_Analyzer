using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.ReferenceBenefits.Constants.ReferenceBenefitsOperationClaims;

namespace Application.Features.ReferenceBenefits.Queries.GetList;

public class GetListReferenceBenefitQuery : IRequest<GetListResponse<GetListReferenceBenefitListItemDto>>, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListReferenceBenefits({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetReferenceBenefits";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListReferenceBenefitQueryHandler : IRequestHandler<GetListReferenceBenefitQuery, GetListResponse<GetListReferenceBenefitListItemDto>>
    {
        private readonly IReferenceBenefitRepository _referenceBenefitRepository;
        private readonly IMapper _mapper;

        public GetListReferenceBenefitQueryHandler(IReferenceBenefitRepository referenceBenefitRepository, IMapper mapper)
        {
            _referenceBenefitRepository = referenceBenefitRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListReferenceBenefitListItemDto>> Handle(GetListReferenceBenefitQuery request, CancellationToken cancellationToken)
        {
            IPaginate<ReferenceBenefit> referenceBenefits = await _referenceBenefitRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListReferenceBenefitListItemDto> response = _mapper.Map<GetListResponse<GetListReferenceBenefitListItemDto>>(referenceBenefits);
            return response;
        }
    }
}
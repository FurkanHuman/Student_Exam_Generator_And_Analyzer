using Application.Services.Personels;
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
using static Application.Features.Principals.Constants.PrincipalsOperationClaims;

namespace Application.Features.Principals.Queries.GetList;

public class GetListPrincipalQuery : IRequest<GetListResponse<GetListPrincipalListItemDto>>, ISecuredRequest, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListPrincipals({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetPrincipals";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListPrincipalQueryHandler : IRequestHandler<GetListPrincipalQuery, GetListResponse<GetListPrincipalListItemDto>>
    {
        private readonly IPrincipalRepository _principalRepository;
        private readonly IMapper _mapper;

        public GetListPrincipalQueryHandler(IPrincipalRepository principalRepository, IMapper mapper)
        {
            _principalRepository = principalRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListPrincipalListItemDto>> Handle(GetListPrincipalQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Principal> principals = await _principalRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                include: p => p.Include(p => p.Personel),
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListPrincipalListItemDto> response = _mapper.Map<GetListResponse<GetListPrincipalListItemDto>>(principals);
            return response;
        }
    }
}
using Application.Features.Personels.Constants;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using MediatR;
using static Application.Features.Personels.Constants.PersonelsOperationClaims;

namespace Application.Features.Personels.Queries.GetList;

public class GetListPersonelQuery : IRequest<GetListResponse<GetListPersonelListItemDto>>, ISecuredRequest, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListPersonels({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetPersonels";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListPersonelQueryHandler : IRequestHandler<GetListPersonelQuery, GetListResponse<GetListPersonelListItemDto>>
    {
        private readonly IPersonelRepository _personelRepository;
        private readonly IMapper _mapper;

        public GetListPersonelQueryHandler(IPersonelRepository personelRepository, IMapper mapper)
        {
            _personelRepository = personelRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListPersonelListItemDto>> Handle(GetListPersonelQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Personel> personels = await _personelRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize, 
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListPersonelListItemDto> response = _mapper.Map<GetListResponse<GetListPersonelListItemDto>>(personels);
            return response;
        }
    }
}
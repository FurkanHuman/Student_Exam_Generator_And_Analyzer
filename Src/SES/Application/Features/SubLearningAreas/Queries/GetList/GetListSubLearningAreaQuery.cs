using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.SubLearningAreas.Queries.GetList;

public class GetListSubLearningAreaQuery : IRequest<GetListResponse<GetListSubLearningAreaListItemDto>>, ICachableRequest
{
    public PageRequest PageRequest { get; set; }



    public bool BypassCache { get; }
    public string? CacheKey => $"GetListSubLearningAreas({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetSubLearningAreas";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListSubLearningAreaQueryHandler : IRequestHandler<GetListSubLearningAreaQuery, GetListResponse<GetListSubLearningAreaListItemDto>>
    {
        private readonly ISubLearningAreaRepository _subLearningAreaRepository;
        private readonly IMapper _mapper;

        public GetListSubLearningAreaQueryHandler(ISubLearningAreaRepository subLearningAreaRepository, IMapper mapper)
        {
            _subLearningAreaRepository = subLearningAreaRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListSubLearningAreaListItemDto>> Handle(GetListSubLearningAreaQuery request, CancellationToken cancellationToken)
        {
            IPaginate<SubLearningArea> subLearningAreas = await _subLearningAreaRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListSubLearningAreaListItemDto> response = _mapper.Map<GetListResponse<GetListSubLearningAreaListItemDto>>(subLearningAreas);
            return response;
        }
    }
}
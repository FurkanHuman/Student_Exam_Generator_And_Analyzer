using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.LearningAreas.Constants.LearningAreasOperationClaims;

namespace Application.Features.LearningAreas.Queries.GetList;

public class GetListLearningAreaQuery : IRequest<GetListResponse<GetListLearningAreaListItemDto>>, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListLearningAreas({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetLearningAreas";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListLearningAreaQueryHandler : IRequestHandler<GetListLearningAreaQuery, GetListResponse<GetListLearningAreaListItemDto>>
    {
        private readonly ILearningAreaRepository _learningAreaRepository;
        private readonly IMapper _mapper;

        public GetListLearningAreaQueryHandler(ILearningAreaRepository learningAreaRepository, IMapper mapper)
        {
            _learningAreaRepository = learningAreaRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListLearningAreaListItemDto>> Handle(GetListLearningAreaQuery request, CancellationToken cancellationToken)
        {
            IPaginate<LearningArea> learningAreas = await _learningAreaRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListLearningAreaListItemDto> response = _mapper.Map<GetListResponse<GetListLearningAreaListItemDto>>(learningAreas);
            return response;
        }
    }
}
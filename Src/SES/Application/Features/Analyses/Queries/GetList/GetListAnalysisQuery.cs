using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Analyses.Constants.AnalysesOperationClaims;

namespace Application.Features.Analyses.Queries.GetList;

public class GetListAnalysisQuery : IRequest<GetListResponse<GetListAnalysisListItemDto>>, ISecuredRequest, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListAnalyses({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetAnalyses";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListAnalysisQueryHandler : IRequestHandler<GetListAnalysisQuery, GetListResponse<GetListAnalysisListItemDto>>
    {
        private readonly IAnalysisRepository _analysisRepository;
        private readonly IMapper _mapper;

        public GetListAnalysisQueryHandler(IAnalysisRepository analysisRepository, IMapper mapper)
        {
            _analysisRepository = analysisRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListAnalysisListItemDto>> Handle(GetListAnalysisQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Analysis> analyses = await _analysisRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListAnalysisListItemDto> response = _mapper.Map<GetListResponse<GetListAnalysisListItemDto>>(analyses);
            return response;
        }
    }
}
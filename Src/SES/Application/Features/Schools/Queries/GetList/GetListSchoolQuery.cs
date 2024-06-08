using Application.Features.Schools.Constants;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using MediatR;
using static Application.Features.Schools.Constants.SchoolsOperationClaims;

namespace Application.Features.Schools.Queries.GetList;

public class GetListSchoolQuery : IRequest<GetListResponse<GetListSchoolListItemDto>>, ISecuredRequest, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListSchools({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetSchools";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListSchoolQueryHandler : IRequestHandler<GetListSchoolQuery, GetListResponse<GetListSchoolListItemDto>>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;

        public GetListSchoolQueryHandler(ISchoolRepository schoolRepository, IMapper mapper)
        {
            _schoolRepository = schoolRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListSchoolListItemDto>> Handle(GetListSchoolQuery request, CancellationToken cancellationToken)
        {
            IPaginate<School> schools = await _schoolRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize, 
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListSchoolListItemDto> response = _mapper.Map<GetListResponse<GetListSchoolListItemDto>>(schools);
            return response;
        }
    }
}
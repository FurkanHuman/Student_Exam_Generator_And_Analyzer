using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Semesters.Constants.SemestersOperationClaims;

namespace Application.Features.Semesters.Queries.GetList;

public class GetListSemesterQuery : IRequest<GetListResponse<GetListSemesterListItemDto>>, ISecuredRequest, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListSemesters({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetSemesters";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListSemesterQueryHandler : IRequestHandler<GetListSemesterQuery, GetListResponse<GetListSemesterListItemDto>>
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly IMapper _mapper;

        public GetListSemesterQueryHandler(ISemesterRepository semesterRepository, IMapper mapper)
        {
            _semesterRepository = semesterRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListSemesterListItemDto>> Handle(GetListSemesterQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Semester> semesters = await _semesterRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListSemesterListItemDto> response = _mapper.Map<GetListResponse<GetListSemesterListItemDto>>(semesters);
            return response;
        }
    }
}
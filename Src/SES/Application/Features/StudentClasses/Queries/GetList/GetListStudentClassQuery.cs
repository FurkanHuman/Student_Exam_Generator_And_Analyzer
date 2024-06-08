using Application.Features.StudentClasses.Constants;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using MediatR;
using static Application.Features.StudentClasses.Constants.StudentClassesOperationClaims;

namespace Application.Features.StudentClasses.Queries.GetList;

public class GetListStudentClassQuery : IRequest<GetListResponse<GetListStudentClassListItemDto>>, ISecuredRequest, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListStudentClasses({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetStudentClasses";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListStudentClassQueryHandler : IRequestHandler<GetListStudentClassQuery, GetListResponse<GetListStudentClassListItemDto>>
    {
        private readonly IStudentClassRepository _studentClassRepository;
        private readonly IMapper _mapper;

        public GetListStudentClassQueryHandler(IStudentClassRepository studentClassRepository, IMapper mapper)
        {
            _studentClassRepository = studentClassRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListStudentClassListItemDto>> Handle(GetListStudentClassQuery request, CancellationToken cancellationToken)
        {
            IPaginate<StudentClass> studentClasses = await _studentClassRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize, 
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListStudentClassListItemDto> response = _mapper.Map<GetListResponse<GetListStudentClassListItemDto>>(studentClasses);
            return response;
        }
    }
}
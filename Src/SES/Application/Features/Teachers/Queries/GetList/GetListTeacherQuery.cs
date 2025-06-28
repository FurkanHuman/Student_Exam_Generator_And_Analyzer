using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Teachers.Queries.GetList;

public class GetListTeacherQuery : IRequest<GetListResponse<GetListTeacherListItemDto>>, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListTeachers({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetTeachers";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListTeacherQueryHandler : IRequestHandler<GetListTeacherQuery, GetListResponse<GetListTeacherListItemDto>>
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;

        public GetListTeacherQueryHandler(ITeacherRepository teacherRepository, IMapper mapper)
        {
            _teacherRepository = teacherRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListTeacherListItemDto>> Handle(GetListTeacherQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Teacher> teachers = await _teacherRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                include: p => p.Include(p => p.Personel),
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListTeacherListItemDto> response = _mapper.Map<GetListResponse<GetListTeacherListItemDto>>(teachers);
            return response;
        }
    }
}
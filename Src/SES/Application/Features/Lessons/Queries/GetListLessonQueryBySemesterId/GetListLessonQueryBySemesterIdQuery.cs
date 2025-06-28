using Application.Features.Lessons.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Lessons.Queries.GetListLessonQueryBySemesterId;

public class GetListLessonQueryBySemesterIdQuery : IRequest<GetListResponse<GetListLessonQueryBySemesterIdDto>>, ICachableRequest, ILoggableRequest
{
    public int SemesterId { get; set; }

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListLessonQueryBySemesterId";
    public string? CacheGroupKey => "GetLessons";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListLessonQueryBySemesterIdQueryHandler : IRequestHandler<GetListLessonQueryBySemesterIdQuery, GetListResponse<GetListLessonQueryBySemesterIdDto>>
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;
        private readonly LessonBusinessRules _lessonBusinessRules;

        public GetListLessonQueryBySemesterIdQueryHandler(ILessonRepository lessonRepository, IMapper mapper, LessonBusinessRules lessonBusinessRules)
        {
            _lessonRepository = lessonRepository;
            _mapper = mapper;
            _lessonBusinessRules = lessonBusinessRules;
        }

        public async Task<GetListResponse<GetListLessonQueryBySemesterIdDto>> Handle(GetListLessonQueryBySemesterIdQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Lesson> lessons = await _lessonRepository.GetListAsync(
                predicate: l => l.SemesterId == request.SemesterId,
                include: l => l.Include(l => l.Semester)
                    .Include(l => l.StudentClasses),
                cancellationToken: cancellationToken);

            GetListResponse<GetListLessonQueryBySemesterIdDto> response = _mapper.Map<GetListResponse<GetListLessonQueryBySemesterIdDto>>(lessons);
            return response;
        }
    }
}

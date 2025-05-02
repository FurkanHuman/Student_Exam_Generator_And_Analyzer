using Application.Features.Lessons.Constants;
using Application.Features.Lessons.Rules;
using Application.Services.Repositories;
using Application.Services.StudentClasses;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Lessons.Constants.LessonsOperationClaims;

namespace Application.Features.Lessons.Commands.Create;

public class CreateLessonCommand : IRequest<CreatedLessonResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required string LessonName { get; set; }
    public required string Description { get; set; }
    public required int SemesterId { get; set; }
    public required int ClassAge { get; set; }

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetLessons"];

    public class CreateLessonCommandHandler : IRequestHandler<CreateLessonCommand, CreatedLessonResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILessonRepository _lessonRepository;
        private readonly IStudentClassService _studentClassService;
        private readonly LessonBusinessRules _lessonBusinessRules;

        public CreateLessonCommandHandler(IMapper mapper, ILessonRepository lessonRepository, IStudentClassService studentClassService, LessonBusinessRules lessonBusinessRules)
        {
            _mapper = mapper;
            _lessonRepository = lessonRepository;
            _studentClassService = studentClassService;
            _lessonBusinessRules = lessonBusinessRules;
        }

        public async Task<CreatedLessonResponse> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
        {
            Lesson lesson = _mapper.Map<Lesson>(request);
            IPaginate<StudentClass>? classes = await _studentClassService.GetListAsync(
                                                                                       predicate: sc => sc.ClassAge == request.ClassAge,
                                                                                       cancellationToken: cancellationToken);

            if (classes != null && classes.Items.Count > 0)
                lesson.StudentClasses = classes.Items;

            await _lessonRepository.AddAsync(lesson);

            CreatedLessonResponse response = _mapper.Map<CreatedLessonResponse>(lesson);
            return response;
        }
    }
}
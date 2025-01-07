using Application.Features.Lessons.Constants;
using Application.Features.Lessons.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Lessons.Constants.LessonsOperationClaims;

namespace Application.Features.Lessons.Commands.Update;

public class UpdateLessonCommand : IRequest<UpdatedLessonResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
    public required string LessonName { get; set; }
    public required string Description { get; set; }
    public required int SemesterId { get; set; }
    public required Semester Semester { get; set; }

    

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetLessons"];

    public class UpdateLessonCommandHandler : IRequestHandler<UpdateLessonCommand, UpdatedLessonResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILessonRepository _lessonRepository;
        private readonly LessonBusinessRules _lessonBusinessRules;

        public UpdateLessonCommandHandler(IMapper mapper, ILessonRepository lessonRepository,
                                         LessonBusinessRules lessonBusinessRules)
        {
            _mapper = mapper;
            _lessonRepository = lessonRepository;
            _lessonBusinessRules = lessonBusinessRules;
        }

        public async Task<UpdatedLessonResponse> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
        {
            Lesson? lesson = await _lessonRepository.GetAsync(predicate: l => l.Id == request.Id, cancellationToken: cancellationToken);
            await _lessonBusinessRules.LessonShouldExistWhenSelected(lesson);
            lesson = _mapper.Map(request, lesson);

            await _lessonRepository.UpdateAsync(lesson!);

            UpdatedLessonResponse response = _mapper.Map<UpdatedLessonResponse>(lesson);
            return response;
        }
    }
}
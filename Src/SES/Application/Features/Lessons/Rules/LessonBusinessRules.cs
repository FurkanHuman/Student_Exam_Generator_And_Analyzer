using Application.Features.Lessons.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.Lessons.Rules;

public class LessonBusinessRules : BaseBusinessRules
{
    private readonly ILessonRepository _lessonRepository;
    private readonly ILocalizationService _localizationService;

    public LessonBusinessRules(ILessonRepository lessonRepository, ILocalizationService localizationService)
    {
        _lessonRepository = lessonRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, LessonsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task LessonShouldExistWhenSelected(Lesson? lesson)
    {
        if (lesson == null)
            await throwBusinessException(LessonsBusinessMessages.LessonNotExists);
    }

    public async Task LessonIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        Lesson? lesson = await _lessonRepository.GetAsync(
            predicate: l => l.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await LessonShouldExistWhenSelected(lesson);
    }
}
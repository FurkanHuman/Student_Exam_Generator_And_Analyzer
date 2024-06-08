using Application.Features.QuizQuestions.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.QuizQuestions.Rules;

public class QuizQuestionBusinessRules : BaseBusinessRules
{
    private readonly IQuizQuestionRepository _quizQuestionRepository;
    private readonly ILocalizationService _localizationService;

    public QuizQuestionBusinessRules(IQuizQuestionRepository quizQuestionRepository, ILocalizationService localizationService)
    {
        _quizQuestionRepository = quizQuestionRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, QuizQuestionsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task QuizQuestionShouldExistWhenSelected(QuizQuestion? quizQuestion)
    {
        if (quizQuestion == null)
            await throwBusinessException(QuizQuestionsBusinessMessages.QuizQuestionNotExists);
    }

    public async Task QuizQuestionIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        QuizQuestion? quizQuestion = await _quizQuestionRepository.GetAsync(
            predicate: qq => qq.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await QuizQuestionShouldExistWhenSelected(quizQuestion);
    }
}
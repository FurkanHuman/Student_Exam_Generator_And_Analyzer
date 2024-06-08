using Application.Features.Exams.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.Exams.Rules;

public class ExamBusinessRules : BaseBusinessRules
{
    private readonly IExamRepository _examRepository;
    private readonly ILocalizationService _localizationService;

    public ExamBusinessRules(IExamRepository examRepository, ILocalizationService localizationService)
    {
        _examRepository = examRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, ExamsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task ExamShouldExistWhenSelected(Exam? exam)
    {
        if (exam == null)
            await throwBusinessException(ExamsBusinessMessages.ExamNotExists);
    }

    public async Task ExamIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        Exam? exam = await _examRepository.GetAsync(
            predicate: e => e.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await ExamShouldExistWhenSelected(exam);
    }
}
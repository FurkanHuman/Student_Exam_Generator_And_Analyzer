using FluentValidation;

namespace Application.Features.QuizQuestions.Commands.Update;

public class UpdateQuizQuestionCommandValidator : AbstractValidator<UpdateQuizQuestionCommand>
{
    public UpdateQuizQuestionCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.BenefitId).NotEmpty();
        RuleFor(c => c.ExamId).NotEmpty();
        RuleFor(c => c.Score).NotEmpty();
        RuleFor(c => c.Prompt).NotEmpty();
        RuleFor(c => c.QuestionImage).NotEmpty();
        RuleFor(c => c.QuestionType).NotEmpty();
    }
}
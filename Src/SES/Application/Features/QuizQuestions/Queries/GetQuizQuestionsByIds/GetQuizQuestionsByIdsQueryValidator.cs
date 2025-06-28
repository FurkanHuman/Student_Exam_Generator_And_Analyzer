using FluentValidation;

namespace Application.Features.QuizQuestions.Queries.GetQuizQuestionsByIds;

public class GetQuizQuestionsByIdsQueryValidator : AbstractValidator<GetQuizQuestionsByIdsQuery>
{
    public GetQuizQuestionsByIdsQueryValidator()
    {
        RuleFor(q => q.Ids)
            .NotEmpty()
            .NotNull()
            .WithMessage("Ids should not be empty.");
    }
}
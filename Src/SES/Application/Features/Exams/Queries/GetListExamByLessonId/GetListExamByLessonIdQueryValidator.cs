using FluentValidation;

namespace Application.Features.Exams.Queries.GetListExamByLessonId;

public class GetListExamByLessonIdQueryValidator : AbstractValidator<GetListExamByLessonIdQuery>
{
    public GetListExamByLessonIdQueryValidator()
    {
        RuleFor(e => e.LessonId)
            .NotEmpty()
            .NotNull()
            .NotEqual(0)
            .WithMessage("Lesson Id must not be empty, null, or zero.");
    }
}
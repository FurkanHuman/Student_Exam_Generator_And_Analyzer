using FluentValidation;

namespace Application.Features.Lessons.Queries.GetListLessonQueryBySemesterId;

public class GetListLessonQueryBySemesterIdQueryValidator : AbstractValidator<GetListLessonQueryBySemesterIdQuery>
{
    public GetListLessonQueryBySemesterIdQueryValidator() 
    {
        RuleFor(x => x.SemesterId)
            .NotEmpty()
            .NotNull()
            .WithMessage("SemesterId cannot be empty or null.");
    }
}
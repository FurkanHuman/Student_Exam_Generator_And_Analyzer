using FluentValidation;

namespace Application.Features.Lessons.Commands.Create;

public class CreateLessonCommandValidator : AbstractValidator<CreateLessonCommand>
{
    public CreateLessonCommandValidator()
    {
        RuleFor(c => c.LessonName).NotEmpty();
        RuleFor(c => c.Description).NotEmpty();
        RuleFor(c => c.SemesterId).NotEmpty();
        RuleFor(c => c.Semester).NotEmpty();
    }
}
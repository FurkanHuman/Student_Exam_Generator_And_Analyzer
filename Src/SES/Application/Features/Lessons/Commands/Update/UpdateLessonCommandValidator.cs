using FluentValidation;

namespace Application.Features.Lessons.Commands.Update;

public class UpdateLessonCommandValidator : AbstractValidator<UpdateLessonCommand>
{
    public UpdateLessonCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.LessonName).NotEmpty();
        RuleFor(c => c.Description).NotEmpty();
        RuleFor(c => c.SemesterId).NotEmpty();
        RuleFor(c => c.Semester).NotEmpty();
    }
}
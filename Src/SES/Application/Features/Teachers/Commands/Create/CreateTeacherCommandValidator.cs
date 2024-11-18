using FluentValidation;

namespace Application.Features.Teachers.Commands.Create;

public class CreateTeacherCommandValidator : AbstractValidator<CreateTeacherCommand>
{
    public CreateTeacherCommandValidator()
    {

        RuleFor(c => c.SemesterId).NotEmpty();
        RuleFor(c => c.SchoolId).NotEmpty();
        RuleFor(c => c.PersonelId).NotEmpty();

    }
}
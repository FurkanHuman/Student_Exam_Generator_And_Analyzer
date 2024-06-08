using FluentValidation;

namespace Application.Features.Semesters.Commands.Create;

public class CreateSemesterCommandValidator : AbstractValidator<CreateSemesterCommand>
{
    public CreateSemesterCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.BeginSemesterDate).NotEmpty();
        RuleFor(c => c.EndSemesterDate).NotEmpty();
    }
}
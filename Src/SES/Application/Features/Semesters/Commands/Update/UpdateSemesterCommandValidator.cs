using FluentValidation;

namespace Application.Features.Semesters.Commands.Update;

public class UpdateSemesterCommandValidator : AbstractValidator<UpdateSemesterCommand>
{
    public UpdateSemesterCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.BeginSemesterDate).NotEmpty();
        RuleFor(c => c.EndSemesterDate).NotEmpty();
    }
}
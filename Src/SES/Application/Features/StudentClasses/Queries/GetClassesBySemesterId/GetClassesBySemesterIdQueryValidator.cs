using FluentValidation;

namespace Application.Features.StudentClasses.Queries.GetClassesBySemesterId;

public class GetClassesBySemesterIdQueryValidator : AbstractValidator<GetClassesBySemesterIdQuery>
{
    public GetClassesBySemesterIdQueryValidator()
    {
        RuleFor(x => x.SemesterId)
            .NotEmpty().NotNull()
            .WithMessage("Semester ID cannot be empty.")
            .GreaterThan(0)
            .WithMessage("Semester ID must be greater than 0.");
    }
}
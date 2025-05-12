using FluentValidation;

namespace Application.Features.StudentClasses.Queries.GetClassesByClassAge;

public class GetClassesByClassAgeQueryValidator : AbstractValidator<GetClassesByClassAgeQuery>
{
    public GetClassesByClassAgeQueryValidator() 
    {
        RuleFor(x => x.ClassAge)
            .NotEmpty()
            .WithMessage("Class age cannot be empty.")
            .GreaterThan(0)
            .WithMessage("Class age must be greater than 0.");
        RuleFor(x => x.SemesterId)
            .NotEmpty()
            .WithMessage("Semester ID cannot be empty.")
            .GreaterThan(0)
            .WithMessage("Semester ID must be greater than 0.");
    }
}
using FluentValidation;

namespace Application.Features.Students.Queries.GetStudentsByIds;

public class GetStudentsByIdsQueryValidator : AbstractValidator<GetStudentsByIdsQuery>
{
    public GetStudentsByIdsQueryValidator()
    {
        RuleFor(query => query.Ids)
            .NotEmpty()
            .NotNull()
            .WithMessage("Ids should not be empty.");
    }
}
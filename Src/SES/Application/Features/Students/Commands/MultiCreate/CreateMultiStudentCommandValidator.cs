using FluentValidation;

namespace Application.Features.Students.Commands.MultiCreate;

class CreateMultiStudentCommandValidator : AbstractValidator<CreateMultiStudentCommand>
{
    public CreateMultiStudentCommandValidator()
    {
        RuleFor(cms => cms.PdfFile).NotEmpty().NotNull();
        RuleFor(cms => cms.SchoolId).NotEmpty().NotNull();
        RuleFor(cms => cms.RefTeacherId).NotEmpty().NotNull();
        RuleFor(cms => cms.SemesterId).NotEmpty().NotNull();
    }
}

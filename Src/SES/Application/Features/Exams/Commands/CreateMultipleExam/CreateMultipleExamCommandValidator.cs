using FluentValidation;

namespace Application.Features.Exams.Commands.CreateMultipleExam;

public class CreateMultipleExamCommandValidator : AbstractValidator<CreateMultipleExamCommand>
{
    public CreateMultipleExamCommandValidator() { }
}
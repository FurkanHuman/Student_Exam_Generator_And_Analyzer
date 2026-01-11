using FluentValidation;

namespace Application.Features.FeedBacks.Commands.Create;

public class CreateFeedBackCommandValidator : AbstractValidator<CreateFeedBackCommand>
{
    public CreateFeedBackCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty();
        RuleFor(c => c.PageUrl).NotEmpty();
        RuleFor(c => c.Message).NotEmpty();
    }
}
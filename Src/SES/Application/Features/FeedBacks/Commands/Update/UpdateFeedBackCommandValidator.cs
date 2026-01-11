using FluentValidation;

namespace Application.Features.FeedBacks.Commands.Update;

public class UpdateFeedBackCommandValidator : AbstractValidator<UpdateFeedBackCommand>
{
    public UpdateFeedBackCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Email).NotEmpty();
        RuleFor(c => c.PageUrl).NotEmpty();
        RuleFor(c => c.Message).NotEmpty();
        RuleFor(c => c.SubmittedAt).NotEmpty();
    }
}
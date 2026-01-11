using FluentValidation;

namespace Application.Features.FeedBacks.Commands.Delete;

public class DeleteFeedBackCommandValidator : AbstractValidator<DeleteFeedBackCommand>
{
    public DeleteFeedBackCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
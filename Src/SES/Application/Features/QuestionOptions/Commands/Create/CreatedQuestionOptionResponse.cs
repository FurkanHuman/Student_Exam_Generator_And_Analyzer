using NArchitecture.Core.Application.Responses;

namespace Application.Features.QuestionOptions.Commands.Create;

public class CreatedQuestionOptionResponse : IResponse
{
    public Guid Id { get; set; }
    public int QuizQuestionId { get; set; }
    public string? OptionText { get; set; }
    public bool IsCorrect { get; set; }
}
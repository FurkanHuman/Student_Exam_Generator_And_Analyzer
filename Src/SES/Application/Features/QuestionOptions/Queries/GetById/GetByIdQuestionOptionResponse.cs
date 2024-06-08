using NArchitecture.Core.Application.Responses;

namespace Application.Features.QuestionOptions.Queries.GetById;

public class GetByIdQuestionOptionResponse : IResponse
{
    public Guid Id { get; set; }
    public int QuizQuestionId { get; set; }
    public string? OptionText { get; set; }
    public bool IsCorrect { get; set; }
}
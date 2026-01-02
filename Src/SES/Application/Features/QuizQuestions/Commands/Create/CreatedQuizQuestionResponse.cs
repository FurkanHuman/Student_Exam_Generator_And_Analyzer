using Domain.Enums;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.QuizQuestions.Commands.Create;

public class CreatedQuizQuestionResponse : IResponse
{
    public int Id { get; set; }
    public int Score { get; set; }
    public string Prompt { get; set; }
    public string? Stem { get; set; }
    public string QuestionImage { get; set; }
    public QuestionType QuestionType { get; set; }
}
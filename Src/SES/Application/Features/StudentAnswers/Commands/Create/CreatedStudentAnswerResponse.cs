using NArchitecture.Core.Application.Responses;

namespace Application.Features.StudentAnswers.Commands.Create;

public class CreatedStudentAnswerResponse : IResponse
{
    public Guid Id { get; set; }
    public int StudentId { get; set; }
    public int QuizQuestionId { get; set; }
    public Guid? QuestionOptionId { get; set; }
    public string? AnswerText { get; set; }
    public int Score { get; set; }
    public bool IsCorrect { get; set; }
}
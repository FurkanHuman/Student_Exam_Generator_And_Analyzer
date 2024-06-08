using NArchitecture.Core.Application.Dtos;

namespace Application.Features.StudentAnswers.Queries.GetList;

public class GetListStudentAnswerListItemDto : IDto
{
    public Guid Id { get; set; }
    public int StudentId { get; set; }
    public int QuizQuestionId { get; set; }
    public Guid? QuestionOptionId { get; set; }
    public string? AnswerText { get; set; }
    public int Score { get; set; }
    public bool IsCorrect { get; set; }
}
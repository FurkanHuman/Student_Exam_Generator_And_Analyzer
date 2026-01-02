using Domain.Enums;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.QuizQuestions.Queries.GetById;

public class GetByIdQuizQuestionResponse : IResponse
{
    public int Id { get; set; }
    public int BenefitId { get; set; }
    public int ExamId { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public string Prompt { get; set; }
    public string? Stem { get; set; }
    public string QuestionImage { get; set; }
    public QuestionType QuestionType { get; set; }
}
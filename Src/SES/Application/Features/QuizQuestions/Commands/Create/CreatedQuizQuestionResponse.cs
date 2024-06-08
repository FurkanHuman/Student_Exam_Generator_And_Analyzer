using NArchitecture.Core.Application.Responses;
using Domain.Enums;

namespace Application.Features.QuizQuestions.Commands.Create;

public class CreatedQuizQuestionResponse : IResponse
{
    public int Id { get; set; }
    public int BenefitId { get; set; }
    public int ExamId { get; set; }
    public int Score { get; set; }
    public string Question { get; set; }
    public string QuestionBody { get; set; }
    public string QuestionImage { get; set; }
    public QuestionType QuestionType { get; set; }
}
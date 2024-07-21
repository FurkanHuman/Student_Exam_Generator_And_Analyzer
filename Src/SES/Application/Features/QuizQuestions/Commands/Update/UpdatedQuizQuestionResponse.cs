using Domain.Enums;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.QuizQuestions.Commands.Update;

public class UpdatedQuizQuestionResponse : IResponse
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
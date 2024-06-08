using NArchitecture.Core.Application.Dtos;
using Domain.Enums;

namespace Application.Features.QuizQuestions.Queries.GetList;

public class GetListQuizQuestionListItemDto : IDto
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
using Domain.Enums;
using NArchitecture.Core.Application.Dtos;

namespace Application.Features.QuizQuestions.Queries.GetList;

public class GetListQuizQuestionListItemDto : IDto
{
    public int Id { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public string Prompt { get; set; }
    public string? Stem { get; set; }
    public string QuestionImage { get; set; }
    public QuestionType QuestionType { get; set; }
}
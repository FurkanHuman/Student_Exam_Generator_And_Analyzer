using Domain.Entities;
using Domain.Enums;
using NArchitecture.Core.Application.Dtos;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.QuizQuestions.Queries.GetByLessonId;

public class GetByLessonIdQuizQuestionListItemDto : IResponse, IDto
{
    public int Id { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public string Question { get; set; }
    public string QuestionBody { get; set; }
    public string QuestionImage { get; set; }
    public QuestionType QuestionType { get; set; }
    public ICollection<QuestionOption> Options { get; set; }
    public ICollection<Benefit> Benefits { get; set; }
}
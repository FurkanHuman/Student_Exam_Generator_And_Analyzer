using NArchitecture.Core.Application.Dtos;

namespace Application.Features.QuestionScores.Queries.GetList;

public class GetListQuestionScoreListItemDto : IDto
{
    public int Id { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
}
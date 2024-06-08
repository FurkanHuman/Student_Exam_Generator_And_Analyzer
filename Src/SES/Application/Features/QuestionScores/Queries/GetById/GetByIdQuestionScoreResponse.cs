using NArchitecture.Core.Application.Responses;

namespace Application.Features.QuestionScores.Queries.GetById;

public class GetByIdQuestionScoreResponse : IResponse
{
    public int Id { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
}
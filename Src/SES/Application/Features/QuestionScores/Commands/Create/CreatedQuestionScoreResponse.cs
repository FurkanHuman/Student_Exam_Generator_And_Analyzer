using NArchitecture.Core.Application.Responses;

namespace Application.Features.QuestionScores.Commands.Create;

public class CreatedQuestionScoreResponse : IResponse
{
    public int Id { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
}
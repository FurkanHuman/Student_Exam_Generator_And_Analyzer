using NArchitecture.Core.Application.Responses;

namespace Application.Features.QuestionScores.Commands.Update;

public class UpdatedQuestionScoreResponse : IResponse
{
    public int Id { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
}
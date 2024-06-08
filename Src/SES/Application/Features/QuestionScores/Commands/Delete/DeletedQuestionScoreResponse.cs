using NArchitecture.Core.Application.Responses;

namespace Application.Features.QuestionScores.Commands.Delete;

public class DeletedQuestionScoreResponse : IResponse
{
    public int Id { get; set; }
}
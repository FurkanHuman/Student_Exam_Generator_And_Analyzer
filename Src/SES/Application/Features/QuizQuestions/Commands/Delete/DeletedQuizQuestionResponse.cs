using NArchitecture.Core.Application.Responses;

namespace Application.Features.QuizQuestions.Commands.Delete;

public class DeletedQuizQuestionResponse : IResponse
{
    public int Id { get; set; }
}
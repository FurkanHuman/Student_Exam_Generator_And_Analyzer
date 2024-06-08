using NArchitecture.Core.Application.Responses;

namespace Application.Features.QuestionOptions.Commands.Delete;

public class DeletedQuestionOptionResponse : IResponse
{
    public Guid Id { get; set; }
}
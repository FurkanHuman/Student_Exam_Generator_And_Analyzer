using NArchitecture.Core.Application.Responses;

namespace Application.Features.StudentAnswers.Commands.Delete;

public class DeletedStudentAnswerResponse : IResponse
{
    public Guid Id { get; set; }
}
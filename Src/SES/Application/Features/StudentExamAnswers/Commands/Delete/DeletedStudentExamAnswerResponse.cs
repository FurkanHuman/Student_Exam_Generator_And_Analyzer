using NArchitecture.Core.Application.Responses;

namespace Application.Features.StudentExamAnswers.Commands.Delete;

public class DeletedStudentExamAnswerResponse : IResponse
{
    public Guid Id { get; set; }
}
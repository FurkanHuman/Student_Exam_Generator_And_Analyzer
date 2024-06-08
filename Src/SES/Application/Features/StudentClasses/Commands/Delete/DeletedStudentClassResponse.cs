using NArchitecture.Core.Application.Responses;

namespace Application.Features.StudentClasses.Commands.Delete;

public class DeletedStudentClassResponse : IResponse
{
    public int Id { get; set; }
}
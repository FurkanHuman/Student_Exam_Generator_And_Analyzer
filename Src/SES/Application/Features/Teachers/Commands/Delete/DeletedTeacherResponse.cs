using NArchitecture.Core.Application.Responses;

namespace Application.Features.Teachers.Commands.Delete;

public class DeletedTeacherResponse : IResponse
{
    public int Id { get; set; }
}
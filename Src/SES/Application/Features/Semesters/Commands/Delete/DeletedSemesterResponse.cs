using NArchitecture.Core.Application.Responses;

namespace Application.Features.Semesters.Commands.Delete;

public class DeletedSemesterResponse : IResponse
{
    public int Id { get; set; }
}
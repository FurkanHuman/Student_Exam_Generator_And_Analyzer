using NArchitecture.Core.Application.Responses;

namespace Application.Features.Schools.Commands.Create;

public class CreatedSchoolResponse : IResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
}
using NArchitecture.Core.Application.Responses;

namespace Application.Features.Personels.Commands.Update;

public class UpdatedPersonelResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public DateOnly BirthDate { get; set; }
}
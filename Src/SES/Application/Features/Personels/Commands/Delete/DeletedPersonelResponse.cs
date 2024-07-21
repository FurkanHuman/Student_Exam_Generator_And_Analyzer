using NArchitecture.Core.Application.Responses;

namespace Application.Features.Personels.Commands.Delete;

public class DeletedPersonelResponse : IResponse
{
    public Guid Id { get; set; }
}
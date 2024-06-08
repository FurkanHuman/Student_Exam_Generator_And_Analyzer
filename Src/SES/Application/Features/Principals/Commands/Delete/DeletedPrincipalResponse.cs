using NArchitecture.Core.Application.Responses;

namespace Application.Features.Principals.Commands.Delete;

public class DeletedPrincipalResponse : IResponse
{
    public int Id { get; set; }
}
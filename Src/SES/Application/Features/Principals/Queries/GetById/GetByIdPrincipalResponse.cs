using NArchitecture.Core.Application.Responses;

namespace Application.Features.Principals.Queries.GetById;

public class GetByIdPrincipalResponse : IResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public int SemesterId { get; set; }
}
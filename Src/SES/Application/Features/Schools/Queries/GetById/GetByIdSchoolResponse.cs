using Domain.Entities;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.Schools.Queries.GetById;

public class GetByIdSchoolResponse : IResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Principal Principal { get; set; }
}
using Domain.Entities;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.Principals.Commands.Create;

public class CreatedPrincipalResponse : IResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public int SemesterId { get; set; }
    public School School { get; set; }
    public Semester Semester { get; set; }
}
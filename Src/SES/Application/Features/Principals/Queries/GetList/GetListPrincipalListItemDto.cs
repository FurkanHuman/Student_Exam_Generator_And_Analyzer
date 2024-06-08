using Domain.Entities;
using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Principals.Queries.GetList;

public class GetListPrincipalListItemDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public int SemesterId { get; set; }
    public School School { get; set; }
    public Semester Semester { get; set; }
}
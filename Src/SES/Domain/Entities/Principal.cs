using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Principal : Entity<int>
{
    public string Name { get; set; }
    public string SurName { get; set; }
    public int SemesterId { get; set; }

    public School School { get; set; }
    public Semester Semester { get; set; }
}


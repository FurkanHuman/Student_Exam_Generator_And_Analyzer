using Entity.Base;

namespace Entity.Entities.Mains;

public class School : Entity<int>
{
    public string? Name { get; set; }

    public required Exam Exam { get; set; }
}


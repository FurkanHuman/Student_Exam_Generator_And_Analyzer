using Entity.Base;

namespace Entity.Entities.Mains;

public class School : Entity<int>
{
    public required string Name { get; set; }
}


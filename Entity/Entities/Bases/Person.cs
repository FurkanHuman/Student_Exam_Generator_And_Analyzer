using Entity.Base;

namespace Entity.Entities.Bases
{
    public class Person : Entity<int>
    {
        public required string Name { get; set; }
        public required string SurName { get; set; }
    }
}

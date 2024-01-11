using Entity.Base;

namespace Entity.Entities.Bases
{
    public class Person : Entity<int>
    {
        public string Name { get; set; }
        public string SurName { get; set; }
    }
}

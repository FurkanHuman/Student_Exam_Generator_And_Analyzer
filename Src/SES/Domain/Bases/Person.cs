using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Bases
{
    public class Person : Entity<int>
    {
        public string Name { get; set; }
        public string SurName { get; set; }
    }
}

using Entity.Entities.Bases;
using Entity.Entities.Infos;

namespace Entity.Entities.Mains;

public class Principal : Person
{
    public int SemesterId { get; set; }

    public School School { get; set; }
    public Semester Semester { get; set; }
}


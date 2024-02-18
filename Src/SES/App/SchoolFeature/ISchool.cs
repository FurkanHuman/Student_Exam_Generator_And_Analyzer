using Entity.Entities.Mains;

namespace App.AddSchool;

public interface ISchool
{
    School AddSchool(ref Exam exam);
}

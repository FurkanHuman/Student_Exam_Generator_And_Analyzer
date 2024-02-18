using App.AddSchool;
using Entity.Entities.Mains;

namespace App.SchoolFeature;

public class SchoolV1 : ISchool
{
    public School AddSchool(ref Exam exam)
    {
        Console.Write("LÜTFEN OKUL ADINI GİRİN: ");
        string? schoolName = Console.ReadLine();
        if (string.IsNullOrEmpty(schoolName))
        {
            Console.WriteLine("Boş değer girildi. Lütfen dikkat edin!!!");
            AddSchool(ref exam);
        }
        return new() { Name = schoolName };
    }
}

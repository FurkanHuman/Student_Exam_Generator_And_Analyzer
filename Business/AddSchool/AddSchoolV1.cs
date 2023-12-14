using Entity.Entities.Mains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.AddSchool;

public class AddSchoolV1 : IAddSchool
{
    public School AddSchool(Exam exam)
    {
        Console.Write("LÜTFEN OKUL ADINI GİRİN: ");
        string? schoolName = Console.ReadLine();
        if (string.IsNullOrEmpty(schoolName)) 
        {
            Console.WriteLine("Boş değer girildi. Lütfen dikkat edin!!!");
            AddSchool(exam);
        }
        return new() { Name = schoolName, Exam = exam };
    }
}

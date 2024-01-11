
using Entity.Entities.Mains;
using System.Text;
using System.Text.Json;

namespace App.AddStudent;

public class AddStudentV1 : IAddStudent
{
    public Student AddStudent()
    {
        throw new NotImplementedException();
    }

    public IList<Student> AddStudents()
    {
        throw new NotImplementedException();
    }

    public IList<Student> AddStudentsForCSVFile(string csvFilePath)
    {

        IList<string> fs = File.ReadAllLines(csvFilePath);
        int count = 1;
        foreach (string fl in fs)
        {

            string[] flParts = fl.Split(";");
            new List<Student>().Add(new Student()
            {
                Id = count,
                SchoolNumber = flParts[0],
                Name = flParts[1],
                SurName = flParts[2],
                ClassAge = int.Parse(flParts[3]),
                ClassBranch = char.Parse(flParts[4])
            });
            count++;
        }
        
        return new List<Student>();
    }

    public IList<Student> AddStudentsLoadFromJsonFile(string JsonFilePath)
    {
        string fs = File.ReadAllText(JsonFilePath);

        return JsonSerializer.Deserialize<IList<Student>>(fs);
    }
}

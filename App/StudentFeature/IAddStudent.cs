
using Entity.Entities.Mains;

namespace App.AddStudent;

public interface IAddStudent
{
    IList<Student> AddStudentsForCSVFile(string csvFilePath);
    IList<Student> AddStudentsLoadFromJsonFile(string JsonFilePath);
    IList<Student> AddStudents();

    Student AddStudent();
}
using Entity.Entities.Mains;

namespace Application.Features.Students.CRUD.Create;

public interface ICreateStudent
{
    Task<bool> CreateStudentsForCSVFile(string csvFilePath);
    Task<bool> CreateStudentsLoadFromJsonFile(string JsonFilePath);
    Task<bool> CreateStudents(IList<CreateStudentDTO> addStudentDTOs);
    void CreateStudent(CreateStudentDTO addStudentDTO);
}
using Application.Repositories;
using AutoMapper;
using Entity.Entities.Mains;
using System.Text.Json;

namespace Application.Features.Students.CRUD.Create;

public class CreateStudentV1 : ICreateStudent
{
    private readonly IStudentRepository _StudentRepository;
    private readonly IMapper _mapper;

    public CreateStudentV1(IStudentRepository studentRepository, IMapper mapper)
    {
        _StudentRepository = studentRepository;
        _mapper = mapper;
    }

    public void CreateStudent(CreateStudentDTO addStudentDTO)
    {
        Student student = _mapper.Map<Student>(addStudentDTO);
        student.SchoolId = 1;
        student.SemesterId = 1;
        _StudentRepository.Add(student);
    }

    public async Task<bool> CreateStudents(IList<CreateStudentDTO> addStudentDTOs)
    {
        IList<Student> students = _mapper.Map<IList<Student>>(addStudentDTOs);

        return !_StudentRepository.AddRangeAsync(students).IsCompletedSuccessfully;
    }

    public Task<bool> CreateStudentsForCSVFile(string csvFilePath)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateStudentsLoadFromJsonFile(string JsonFilePath)
    {
        string fs = await File.ReadAllTextAsync(JsonFilePath);
        if (string.IsNullOrWhiteSpace(fs) || string.IsNullOrEmpty(fs))
            return false;

        IList<CreateStudentDTO>? createStudentDTOs = JsonSerializer.Deserialize<IList<CreateStudentDTO>>(fs);

        IList<Student> students = _mapper.Map<IList<Student>>(createStudentDTOs);

        return _StudentRepository.AddRangeAsync(students).IsCompletedSuccessfully;
    }
}

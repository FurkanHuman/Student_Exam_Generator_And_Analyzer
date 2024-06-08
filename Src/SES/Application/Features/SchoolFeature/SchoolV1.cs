using Application.Repositories;
using Entity.Entities.Mains;

namespace Application.Features.SchoolFeature;

public class SchoolV1 : ISchoolService
{
    private readonly ISchoolRepository _SchoolRepository;

    public SchoolV1(ISchoolRepository schoolRepository) => _SchoolRepository = schoolRepository;


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

    public void AddSchool(string schoolName) => _SchoolRepository.Add(new() { Name = schoolName.ToUpper() });


    public string GetSchoolName()
    {
        return _SchoolRepository.GetList().Items.LastOrDefault().Name;

    }

    public Dictionary<int, string> GetSchoolNames()
        => _SchoolRepository.GetList(size: int.MaxValue,
                                                         orderBy: n => n.OrderBy(s => s.Id),
                                                         withDeleted: true).Items
        .ToDictionary(s => s.Id, s => s.Name);

    public bool UpdateSchoolName(string schoolName, int schoolId)
    {
        School? school = _SchoolRepository.Get(s => s.Id == schoolId);
        school.Name = schoolName;
        return _SchoolRepository.UpdateAsync(school).IsCompletedSuccessfully;
    }

    public int GetLastId() => _SchoolRepository.GetList().Items.LastOrDefault().Id;

    public bool IsDbEmpty() => _SchoolRepository.Any();

}

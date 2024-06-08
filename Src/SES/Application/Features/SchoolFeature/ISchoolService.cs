using Entity.Entities.Mains;

namespace Application.Features.SchoolFeature;

public interface ISchoolService
{
    void AddSchool(string schoolName);
    bool UpdateSchoolName(string schoolName, int schoolId);
    bool IsDbEmpty();
    string GetSchoolName();
    Dictionary<int, string> GetSchoolNames();
    int GetLastId();
    School AddSchool(ref Exam exam);
}

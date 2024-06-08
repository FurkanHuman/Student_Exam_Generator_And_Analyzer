using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SemesterFeature.CRUD;
public interface ISemesterService
{
    void AddSemester(CreateSemesterDTO createSemesterDTO);
    void UpdateSemester(UpdateSemesterDTO createSemesterDTO);

    IList<RequestSemesterDTO> GetRequestSemesters();
}

public class UpdateSemesterDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly BeginSemesterDate { get; set; }

    public DateOnly EndSemesterDate { get; set; }
}
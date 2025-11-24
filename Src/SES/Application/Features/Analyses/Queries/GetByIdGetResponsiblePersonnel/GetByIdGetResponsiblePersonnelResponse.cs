using NArchitecture.Core.Application.Responses;

namespace Application.Features.Analyses.Queries.GetByIdGetResponsiblePersonnel;

public class GetByIdGetResponsiblePersonnelResponse:IResponse
{
    public string Principal { get; set; }
    public string ExamAuthorTeacher { get; set; }
    public string[] ReviewingTeachersNames { get; set; }
}

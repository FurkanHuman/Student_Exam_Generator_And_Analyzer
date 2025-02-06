using NArchitecture.Core.Application.Dtos;

namespace Application.Features.ReferenceBenefits.Queries.GetList;

public class GetListReferenceBenefitListItemDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int SemesterId { get; set; }
    public string SemesterName { get; set; }
    public DateOnly BeginSemesterDate { get; set; }
    public DateOnly EndSemesterDate { get; set; }

    public int SchoolId { get; set; }
    public string SchoolName { get; set; }

    public int LessonId { get; set; }
    public string LessonName { get; set; }

}
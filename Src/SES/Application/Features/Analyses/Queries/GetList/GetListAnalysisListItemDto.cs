using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Analyses.Queries.GetList;

public class GetListAnalysisListItemDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LessonName { get; set; }
    public string SemesterName { get; set; }
    public DateOnly SemesterStartDate { get; set; }
    public DateOnly SemesterEndDate { get; set; }
}
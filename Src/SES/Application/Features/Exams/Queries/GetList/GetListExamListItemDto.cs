using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Exams.Queries.GetList;

public class GetListExamListItemDto : IDto
{
    public int Id { get; set; }
    public string LessonName { get; set; }
    public string ExamCode { get; set; }
    public string FooterNote { get; set; }
    public int? TotalScore { get; set; }
    public string? TotalScoreForString { get; set; }
    public int SemesterId { get; set; }
    public int AnalysisId { get; set; }
    public int TeacherId { get; set; }
    public int StudentId { get; set; }
    public int SchoolId { get; set; }
    public int ReferenceBenefitId { get; set; }
}
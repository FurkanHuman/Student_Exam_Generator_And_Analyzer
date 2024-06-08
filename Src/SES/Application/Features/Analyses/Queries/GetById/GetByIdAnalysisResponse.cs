using NArchitecture.Core.Application.Responses;

namespace Application.Features.Analyses.Queries.GetById;

public class GetByIdAnalysisResponse : IResponse
{
    public int Id { get; set; }
    public int ClassAge { get; set; }
    public char AltClass { get; set; }
    public string ExamSemesterYear { get; set; }
    public string LessonName { get; set; }
    public string LessonSession { get; set; }
    public string ExamCode { get; set; }
    public string FooterNote { get; set; }
    public int SemesterId { get; set; }
    public int BenefitId { get; set; }
    public int QuestionId { get; set; }
    public int TeacherId { get; set; }
    public int PrincipalId { get; set; }
    public int StudentAnswerId { get; set; }
    public int SchoolId { get; set; }

}
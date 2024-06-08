using Domain.Entities;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.Exams.Commands.Create;

public class CreatedExamResponse : IResponse
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
    public Semester Semester { get; set; }
    public Teacher Teacher { get; set; }
    public Student Student { get; set; }
    public School School { get; set; }
    public ReferenceBenefit ReferenceBenefit { get; set; }
}
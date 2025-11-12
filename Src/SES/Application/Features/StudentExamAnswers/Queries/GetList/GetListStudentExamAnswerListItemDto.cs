using Domain.Enums;
using NArchitecture.Core.Application.Dtos;

namespace Application.Features.StudentExamAnswers.Queries.GetList;

public class GetListStudentExamAnswerListItemDto : IDto
{
    public Guid Id { get; set; }
    public int ReviewerTeacherId { get; set; }
    public int StudentId { get; set; }
    public int ExamId { get; set; }
    public EvaluationOrigin EvaluationOrigin { get; set; }
    public ExamEvaluationStatus ExamEvaluationStatus { get; set; }
}
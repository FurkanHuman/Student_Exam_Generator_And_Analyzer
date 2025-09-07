using NArchitecture.Core.Application.Responses;
using Domain.Enums;
using Domain.Enums;

namespace Application.Features.StudentExamAnswers.Queries.GetById;

public class GetByIdStudentExamAnswerResponse : IResponse
{
    public Guid Id { get; set; }
    public int ReviewerTeacherId { get; set; }
    public int StudentId { get; set; }
    public int ExamId { get; set; }
    public EvaluationOrigin EvaluationOrigin { get; set; }
    public ExamEvaluationStatus ExamEvaluationStatus { get; set; }
}
using Domain.Enums;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.StudentExamAnswers.Commands.Create;

public class CreatedStudentExamAnswerResponse : IResponse
{
    public Guid Id { get; set; }
    public int ReviewerTeacherId { get; set; }
    public int StudentId { get; set; }
    public int ExamId { get; set; }
    public EvaluationOrigin EvaluationOrigin { get; set; }
    public ExamEvaluationStatus ExamEvaluationStatus { get; set; }
}
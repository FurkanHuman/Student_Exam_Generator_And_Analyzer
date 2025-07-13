namespace Application.Features.StudentAnswers.Commands.CreateMultiple;

public class MultipleStudentAnswer
{
    public int StudentId { get; set; }
    public int ExamId { get; set; }
    public int QuizQuestionId { get; set; }
    public Guid? QuestionOptionId { get; set; }
    public string? AnswerText { get; set; }
    public int? GivenScore { get; set; }
    public byte EvaluationOrigin { get; set; }
    public byte EvaluationStatus { get; set; }
}

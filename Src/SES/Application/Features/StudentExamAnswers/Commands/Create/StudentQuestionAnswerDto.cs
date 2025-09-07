namespace Application.Features.StudentExamAnswers.Commands.Create;

public class StudentQuestionAnswerDto
{
    public int QuizQuestionId { get; set; }
    public Guid? QuestionOptionId { get; set; }
    public string? AnswerText { get; set; }
    public int? GivenScore { get; set; }
    public byte EvaluationStatus { get; set; }

}

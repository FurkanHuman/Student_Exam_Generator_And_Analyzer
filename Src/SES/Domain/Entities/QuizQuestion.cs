using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class QuizQuestion : Entity<int>
{
    public string? Stem { get; set; } // context, story, instruction
    public string Prompt { get; set; } // the actual question to be answered
    public string? QuestionImageURL { get; set; }
    public QuestionType QuestionType { get; set; }
    public bool IsAIGenerated { get; set; } = false;
    public int? PreviousQuestionId { get; set; }
    public QuestionScore QuestionScore { get; set; }
    public QuizQuestion? PreviousQuestion { get; set; }
    public IList<Exam> Exams { get; set; }
    public IList<Benefit> Benefits { get; set; }
    public IList<QuestionOption> Options { get; set; }
    public IList<StudentAnswer> StudentAnswers { get; set; }
    public IList<Lesson> Lessons { get; set; }
}

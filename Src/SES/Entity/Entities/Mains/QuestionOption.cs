using NArchitecture.Core.Persistence.Repositories;

namespace Entity.Entities.Mains;

public class QuestionOption : Entity<Guid> // soruların seçenekleri
{
    public int QuizQuestionId { get; set; }
    public string? OptionText { get; set; }
    public bool IsCorrect { get; set; }
    public virtual QuizQuestion QuizQuestion { get; set; }
}

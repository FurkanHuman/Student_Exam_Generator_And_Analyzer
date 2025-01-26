using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class QuestionOption : Entity<Guid> // soruların seçenekleri
{
    public string? OptionText { get; set; }
    public bool IsCorrect { get; set; }
    public virtual QuizQuestion QuizQuestion { get; set; }
}

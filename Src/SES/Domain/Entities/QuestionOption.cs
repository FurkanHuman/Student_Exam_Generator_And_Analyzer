using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class QuestionOption : Entity<Guid>
{
    public string? OptionText { get; set; }
    public bool IsCorrect { get; set; }
    public virtual QuizQuestion QuizQuestion { get; set; }
}

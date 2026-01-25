using Domain.Common;
using Domain.Shared.ValueObjects;

namespace Domain.LearningArea;

public sealed class SubLearningArea : Entity<int>
{
    public CodeDescription Info { get; private set; }

    private readonly List<Benefit> _benefits = [];
    public IReadOnlyList<Benefit> Benefits => _benefits;

    private SubLearningArea()
    {
        Info = CodeDescription.Create("DEFAULT", "Default");
    }

    public static SubLearningArea Create(string code, string description)
    {
        return new SubLearningArea
        {
            Info = CodeDescription.Create(code, description)
        };
    }

    public void AddBenefit(Benefit benefit)
    {
        if (_benefits.Any(b => b.Info.Code == benefit.Info.Code))
            throw new DomainException("Benefit code already exists");
        _benefits.Add(benefit);
    }
}

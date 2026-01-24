using Domain.Common;
using Domain.Shared.ValueObjects;

namespace Domain.LearningArea;

public sealed class LearningArea : Entity<int>
{
    public CodeDescription Info { get; private set; }

    private readonly List<SubLearningArea> _subAreas = [];
    public IReadOnlyCollection<SubLearningArea> SubAreas => _subAreas;

    private LearningArea()
    {
        Info = CodeDescription.Create("DEFAULT", "Default");
    }

    public static LearningArea Create(string code, string description)
    {
        return new LearningArea
        {
            Info = CodeDescription.Create(code, description)
        };
    }

    public void AddSubArea(SubLearningArea subArea)
    {
        if (_subAreas.Any(s => s.Info.Code == subArea.Info.Code))
            throw new DomainException("Sub-area code already exists");
        _subAreas.Add(subArea);
    }
}

using Domain.Common;
using Domain.Shared.ValueObjects;

namespace Domain.LearningArea;

public sealed class Benefit : Entity<int>
{
    public CodeDescription Info { get; private set; }

    private Benefit()
    {
        Info = CodeDescription.Create("DEFAULT", "Default");
    }

    public static Benefit Create(string code, string description)
    {
        return new Benefit
        {
            Info = CodeDescription.Create(code, description)
        };
    }
}

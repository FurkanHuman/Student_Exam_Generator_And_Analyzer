using Domain.Common;

namespace Domain.School;

public sealed class School : Entity<int>
{
    public string Name { get; private set; }
    public int PrincipalId { get; private set; }

    private School()
    {
        Name = string.Empty;
    }

    public static School Create(string name, int principalId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("School name cannot be empty");
        if (name.Length < 3 || name.Length > 200)
            throw new DomainException("School name must be between 3 and 200 characters");

        return new School
        {
            Name = name.Trim(),
            PrincipalId = principalId
        };
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("School name cannot be empty");
        Name = name.Trim();
    }

    public void ChangePrincipal(int newPrincipalId)
    {
        if (newPrincipalId <= 0)
            throw new DomainException("Invalid principal ID");
        PrincipalId = newPrincipalId;
    }
}

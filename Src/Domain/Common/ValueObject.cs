namespace Domain.Common;

public abstract record ValueObject
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public virtual bool Equals(ValueObject? other)
    {
        if (other is null || GetType() != other.GetType())
            return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        HashCode hash = new();

        foreach (object? component in GetEqualityComponents())
            hash.Add(component);

        return hash.ToHashCode();

    }
}

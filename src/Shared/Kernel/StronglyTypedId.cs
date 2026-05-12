namespace PDH.Shared.Kernel;

public abstract record StronglyTypedId<TValue>(TValue Value)
{
    public override string ToString() => Value?.ToString() ?? string.Empty;
}

namespace PDH.Shared.Kernel;

public record ActivityId(Guid Value) : StronglyTypedId<Guid>(Value);

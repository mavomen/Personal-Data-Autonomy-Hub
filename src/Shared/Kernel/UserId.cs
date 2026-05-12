namespace PDH.Shared.Kernel;

public record UserId(Guid Value) : StronglyTypedId<Guid>(Value);

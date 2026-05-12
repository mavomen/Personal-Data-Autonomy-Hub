namespace PDH.Shared.Kernel;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}

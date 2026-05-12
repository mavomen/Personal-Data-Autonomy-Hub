namespace PDH.Shared.Infrastructure.Outbox;

public class OutboxMessage
{
    public Guid Id { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string Data { get; private set; } = string.Empty;
    public DateTime OccurredOn { get; private set; }
    public DateTime? ProcessedOn { get; private set; }

    private OutboxMessage() { }

    public OutboxMessage(string type, string data, DateTime occurredOn)
    {
        Id = Guid.NewGuid();
        Type = type;
        Data = data;
        OccurredOn = occurredOn;
    }

    public void MarkProcessed()
    {
        ProcessedOn = DateTime.UtcNow;
    }
}

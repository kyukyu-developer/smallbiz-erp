namespace ERP.Shared.Contracts.Common;

public abstract class IntegrationEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
    public string EventType => GetType().Name;
}

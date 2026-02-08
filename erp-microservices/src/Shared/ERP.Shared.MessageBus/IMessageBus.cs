using ERP.Shared.Contracts.Common;

namespace ERP.Shared.MessageBus;

public interface IMessageBus
{
    void Publish<T>(T @event) where T : IntegrationEvent;
    void Subscribe<T>(Func<T, Task> handler) where T : IntegrationEvent;
}

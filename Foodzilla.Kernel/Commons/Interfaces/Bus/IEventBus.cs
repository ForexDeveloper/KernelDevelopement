namespace Foodzilla.Kernel.Commons.Interfaces.Bus;

public interface IEventBus
{
    void Publish(IEvent @event);

    void Subscribe<T>() where T : IEvent;
}
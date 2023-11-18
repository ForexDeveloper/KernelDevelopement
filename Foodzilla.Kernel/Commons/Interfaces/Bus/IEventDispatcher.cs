using Foodzilla.Kernel.Commons.Interfaces.Dependencies;

namespace Foodzilla.Kernel.Commons.Interfaces.Bus;

public interface IEventDispatcher : ISingleton
{
    Task Dispatch<TEvent>(TEvent e) where TEvent : IEvent;
}
using Foodzilla.Kernel.Commons.Interfaces.Dependencies;

namespace Foodzilla.Kernel.Commons.Interfaces.Bus;

public interface IEventHandler<in TEvent> where TEvent : IEvent, IScoped
{
    Task Handle(TEvent e);
}
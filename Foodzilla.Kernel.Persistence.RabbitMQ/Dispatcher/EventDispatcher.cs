using Autofac;
using Foodzilla.Kernel.Commons.Interfaces.Bus;

namespace Foodzilla.Kernel.Persistence.RabbitMQ.Dispatcher;

public sealed class EventDispatcher : IEventDispatcher
{
    private readonly IComponentContext _componentContext;

    public EventDispatcher(IComponentContext componentContext)
    {
        _componentContext = componentContext;
    }

    public Task Dispatch<TEvent>(TEvent e) where TEvent : IEvent
    {
        if (e == null)
        {
            throw new ArgumentNullException(nameof(e));
        }

        var eventType = typeof(IEventHandler<>).MakeGenericType(e.GetType());

        dynamic handler = _componentContext.Resolve(eventType);

        return (Task)eventType
            .GetMethod(nameof(IEventHandler<TEvent>.Handle))!
            .Invoke(handler, new object[] { e });
    }
}
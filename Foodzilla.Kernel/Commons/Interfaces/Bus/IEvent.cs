using Foodzilla.Kernel.Commons.Interfaces.Dependencies;

namespace Foodzilla.Kernel.Commons.Interfaces.Bus;

public interface IEvent : IScoped
{
    Guid Id { get; set; }

    string Name { get; set; }

    DateTime OccurredOn { get; set; }
}
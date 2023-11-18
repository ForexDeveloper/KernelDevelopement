namespace Foodzilla.Kernel.Commons.Interfaces.Bus;

public interface IPersistentConnection<out TModel> : IDisposable
{
    bool IsConnected { get; }

    bool TryConnect();

    TModel CreateModel();
}
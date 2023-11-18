using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foodzilla.Kernel;

public abstract class StartupModuleBase
{
    internal readonly int Order;

    protected StartupModuleBase()
    {
        Order = 100;
    }

    protected StartupModuleBase(int order)
    {
        Order = order;
    }

    public abstract void Execute(IServiceCollection services, IConfiguration configuration);
}
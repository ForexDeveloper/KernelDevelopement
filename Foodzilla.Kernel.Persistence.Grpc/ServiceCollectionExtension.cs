using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foodzilla.Kernel.Persistence.Grpc;

public static class ServiceCollectionExtension
{
    public static void AddGrpcServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpc();
    }
}
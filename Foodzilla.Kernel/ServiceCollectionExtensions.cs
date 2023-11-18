using System.Net.NetworkInformation;
using System.Reflection;
using Foodzilla.Kernel.Commons.Interfaces.Dependencies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foodzilla.Kernel;

public static class ServiceCollectionExtensions
{
    public static void ScanInjections(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromApplicationDependencies()

            .AddClasses(classes => classes.AssignableTo<ISingleton>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime()

            .AddClasses(classes => classes.AssignableTo<IScoped>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()

            .AddClasses(classes => classes.AssignableTo<ITransient>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }

    public static void InjectKernelDependencies(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }

    public static void InjectStartupModules(this IServiceCollection services, IConfiguration configuration/*, string applicationName*/)
    {
        var startupModules = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)));
        //.Where(x => x.FullName.StartsWith($"{applicationName}."))
        var x = startupModules.SelectMany(s => s.GetTypes())
            .Where(p => typeof(StartupModuleBase).IsAssignableFrom(p) && !p.IsAbstract && p.IsClass);

        var startupModuleInstances = new List<StartupModuleBase>();

        foreach (var startupModule in x)
        {
            var instance = Activator.CreateInstance(startupModule);
            if (instance is StartupModuleBase startupModuleInstance)
                startupModuleInstances.Add(startupModuleInstance);
        }

        startupModuleInstances = startupModuleInstances.OrderBy(p => p.Order).ToList();

        foreach (var startupModuleInstance in startupModuleInstances)
        {
            startupModuleInstance.Execute(services, configuration);
        }
    }
}
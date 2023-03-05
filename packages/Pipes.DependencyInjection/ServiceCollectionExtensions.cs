using Microsoft.Extensions.DependencyInjection;
using Pipes.DependencyInjection.Caching;

namespace Pipes.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Add<TInput, TOutput>(this IServiceCollection serviceCollection,
        ServicePipe<TInput, TOutput> servicePipe, ServiceLifetime serviceLifetime)
    {
        if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
        if (servicePipe == null) throw new ArgumentNullException(nameof(servicePipe));

        serviceCollection.AddScoped<ServiceCacheHandler>();

        foreach (var pipeable in servicePipe)
        {
            if (pipeable is not PipeableType p) continue;

            var serviceDescriptor = new ServiceDescriptor(p.Type, p.Type, serviceLifetime);
            serviceCollection.Add(serviceDescriptor);
        }

        servicePipe.ServiceLifetime = serviceLifetime;

        return serviceCollection;
    }

    public static IServiceCollection AddTransient<TInput, TOutput>(this IServiceCollection serviceCollection,
        ServicePipe<TInput, TOutput> servicePipe)
    {
        return Add(serviceCollection, servicePipe, ServiceLifetime.Transient);
    }

    public static IServiceCollection AddScoped<TInput, TOutput>(this IServiceCollection serviceCollection,
        ServicePipe<TInput, TOutput> servicePipe)
    {
        return Add(serviceCollection, servicePipe, ServiceLifetime.Scoped);
    }

    public static IServiceCollection AddSingleton<TInput, TOutput>(this IServiceCollection serviceCollection,
        ServicePipe<TInput, TOutput> servicePipe)
    {
        return Add(serviceCollection, servicePipe, ServiceLifetime.Singleton);
    }
}
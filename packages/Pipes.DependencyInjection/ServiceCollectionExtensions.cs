using Microsoft.Extensions.DependencyInjection;
using Pipes.DependencyInjection.Caching;

namespace Pipes.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Add<TInput, TOutput>(this IServiceCollection serviceCollection,
        Pipe<TInput, TOutput> pipe, ServiceLifetime serviceLifetime)
    {
        if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
        if (pipe == null) throw new ArgumentNullException(nameof(pipe));

        if (pipe is not ServicePipe<TInput, TOutput> servicePipe) return serviceCollection;

        serviceCollection.AddScoped<ServiceCacheHandler>();

        foreach (var pipeable in servicePipe)
        {
            if (pipeable is not PipeableService pipeableService) continue;

            serviceCollection.Add(new ServiceDescriptor(pipeableService.Type, pipeableService.Type, serviceLifetime));
            pipeableService.ServiceLifetime = serviceLifetime;
        }

        serviceCollection.Add(new ServiceDescriptor(typeof(IServicePipe), servicePipe));
        servicePipe.ServiceLifetime = serviceLifetime;

        return serviceCollection;
    }

    public static IServiceCollection AddTransient<TInput, TOutput>(this IServiceCollection serviceCollection,
        Pipe<TInput, TOutput> pipe)
    {
        return Add(serviceCollection, pipe, ServiceLifetime.Transient);
    }

    public static IServiceCollection AddScoped<TInput, TOutput>(this IServiceCollection serviceCollection,
        Pipe<TInput, TOutput> pipe)
    {
        return Add(serviceCollection, pipe, ServiceLifetime.Scoped);
    }

    public static IServiceCollection AddSingleton<TInput, TOutput>(this IServiceCollection serviceCollection,
        Pipe<TInput, TOutput> pipe)
    {
        return Add(serviceCollection, pipe, ServiceLifetime.Singleton);
    }
}
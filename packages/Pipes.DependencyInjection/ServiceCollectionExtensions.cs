using Microsoft.Extensions.DependencyInjection;
using Pipes.Caching;
using Pipes.DependencyInjection.Caching;

namespace Pipes.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Add<TInput, TOutput>(this IServiceCollection serviceCollection,
        ServicePipe<TInput, TOutput> servicePipe, ServiceLifetime serviceLifetime)
    {
        if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
        if (servicePipe == null) throw new ArgumentNullException(nameof(servicePipe));

        // serviceCollection.AddScoped<>();
        
        foreach (var pipeable in servicePipe)
        {
            var serviceDescriptor = pipeable switch
            {
                PipeableType p => new ServiceDescriptor(p.Type, p.Type, serviceLifetime),
                // PipeableCache<object, object> { Pipeable: PipeableType s } => new ServiceDescriptor(s.Type, s.Type,
                //     serviceLifetime),
                _ => null
            };

            if (serviceDescriptor != null) serviceCollection.Add(serviceDescriptor);
        }

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
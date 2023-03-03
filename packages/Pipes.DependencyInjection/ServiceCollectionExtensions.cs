using Microsoft.Extensions.DependencyInjection;

namespace Pipes.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Add<TInput, TOutput>(this IServiceCollection serviceCollection,
        ServicePipe<TInput, TOutput> servicePipe, ServiceLifetime serviceLifetime)
    {
        if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
        if (servicePipe == null) throw new ArgumentNullException(nameof(servicePipe));

        foreach (var service in servicePipe)
        {
            if (service is not PipeableType pipeable) continue;
            var serviceDescriptor = new ServiceDescriptor(pipeable.Type, pipeable.Type, serviceLifetime);
            serviceCollection.Add(serviceDescriptor);
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
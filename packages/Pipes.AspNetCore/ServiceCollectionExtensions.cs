using Microsoft.Extensions.DependencyInjection;

namespace Pipes.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static PipeBuilder AddPipes(this IServiceCollection serviceCollection)
    {
        if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

        var pipeBuilder = new PipeBuilder(serviceCollection);
        
        serviceCollection.AddScoped<ServiceActivationMiddleware>();

        return pipeBuilder;
    }
}
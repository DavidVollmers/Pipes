using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Pipes.AspNetCore;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UsePipes(this IApplicationBuilder applicationBuilder)
    {
        if (applicationBuilder == null) throw new ArgumentNullException(nameof(applicationBuilder));

        applicationBuilder.UseMiddleware<ServiceActivationMiddleware>();

        var pipeBuilder = applicationBuilder.ApplicationServices.GetRequiredService<PipeBuilder>();
        foreach (var servicePipe in pipeBuilder.ServicePipes)
        {
            if (servicePipe is { Activated: false, ServiceLifetime: ServiceLifetime.Singleton })
                servicePipe.Activate(applicationBuilder.ApplicationServices);
        }

        return applicationBuilder;
    }
}
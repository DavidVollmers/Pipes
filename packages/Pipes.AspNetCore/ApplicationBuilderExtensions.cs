using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pipes.DependencyInjection;

namespace Pipes.AspNetCore;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UsePipes(this IApplicationBuilder applicationBuilder)
    {
        if (applicationBuilder == null) throw new ArgumentNullException(nameof(applicationBuilder));

        applicationBuilder.UseMiddleware<ServiceActivationMiddleware>();

        var servicePipes = applicationBuilder.ApplicationServices.GetService<IEnumerable<IServicePipe>>();
        if (servicePipes != null)
        {
            foreach (var servicePipe in servicePipes)
            {
                if (servicePipe is { Activated: false, ServiceLifetime: ServiceLifetime.Singleton })
                    servicePipe.Activate(applicationBuilder.ApplicationServices);
            }
        }

        return applicationBuilder;
    }
}
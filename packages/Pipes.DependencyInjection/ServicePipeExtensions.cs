using Microsoft.Extensions.DependencyInjection;

namespace Pipes.DependencyInjection;

public static class ServicePipeExtensions
{
    public static void EnsureScopeActivation(this IServicePipe servicePipe, IServiceProvider serviceProvider)
    {
        if (servicePipe == null) throw new ArgumentNullException(nameof(servicePipe));
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        if (!servicePipe.Activated)
        {
            servicePipe.Activate(serviceProvider);
            return;
        }

        foreach (var service in servicePipe)
        {
            if (service.ServiceLifetime != ServiceLifetime.Scoped || service.Activated) continue;

            service.Activate(serviceProvider, service.ServiceInjection);
        }
    }

    public static void EnsureScopeReset(this IServicePipe servicePipe, IServiceProvider serviceProvider)
    {
        if (servicePipe == null) throw new ArgumentNullException(nameof(servicePipe));
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        if (servicePipe.ServiceLifetime == ServiceLifetime.Scoped)
        {
            servicePipe.Reset();
            return;
        }

        foreach (var service in servicePipe)
        {
            if (service.ServiceLifetime != ServiceLifetime.Scoped) continue;

            service.Reset();
        }
    }
}
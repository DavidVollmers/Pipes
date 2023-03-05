using Microsoft.Extensions.DependencyInjection;

namespace Pipes.DependencyInjection;

public interface IPipeableService
{
    bool Activated { get; }

    ServiceInjection ServiceInjection { get; }

    ServiceLifetime ServiceLifetime { get; }

    void Activate(IServiceProvider serviceProvider, ServiceInjection serviceInjection);

    void Reset();
}
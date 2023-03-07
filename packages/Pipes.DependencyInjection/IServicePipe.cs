using Microsoft.Extensions.DependencyInjection;

namespace Pipes.DependencyInjection;

public interface IServicePipe : IEnumerable<IPipeableService>
{
    bool Activated { get; }

    ServiceInjection ServiceInjection { get; }

    ServiceLifetime ServiceLifetime { get; }

    void Activate(IServiceProvider serviceProvider);

    void Reset();
}
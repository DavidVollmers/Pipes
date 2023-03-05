using Microsoft.Extensions.DependencyInjection;

namespace Pipes.DependencyInjection;

public interface IServiceActivation
{
    bool Activated { get; }
    
    ServiceLifetime ServiceLifetime { get; }
    
    void Activate(IServiceProvider serviceProvider);

    void Reset();
}
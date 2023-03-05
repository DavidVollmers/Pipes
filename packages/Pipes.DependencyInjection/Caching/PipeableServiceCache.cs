using Microsoft.Extensions.DependencyInjection;
using Pipes.Abstractions;
using Pipes.Caching;

namespace Pipes.DependencyInjection.Caching;

internal class PipeableServiceCache : PipeableService, IPipeableCache
{
    public PipeableServiceCache(Type type, CacheFlags cacheFlags) : base(type)
    {
        CacheFlags = cacheFlags;

        ServiceLifetime = ServiceLifetime.Scoped;
    }

    public CacheFlags CacheFlags { get; }

    public override object? ConvertInput(object? input)
    {
        var cacheHandler = ServiceProvider!.GetService<ServiceCacheHandler>();

        return cacheHandler!.ConvertInput(this, input);
    }

    public override void Execute(IPipe<object, object?> pipe)
    {
        var cacheHandler = ServiceProvider!.GetService<ServiceCacheHandler>();

        cacheHandler!.Execute(this, pipe);
    }

    public override Task ExecuteAsync(IPipe<object, object?> pipe, CancellationToken cancellationToken = default)
    {
        var cacheHandler = ServiceProvider!.GetService<ServiceCacheHandler>();

        return cacheHandler!.ExecuteAsync(this, pipe, cancellationToken);
    }

    public void Clear()
    {
        var cacheHandler = ServiceProvider!.GetService<ServiceCacheHandler>();

        cacheHandler!.Clear(this);
    }
}
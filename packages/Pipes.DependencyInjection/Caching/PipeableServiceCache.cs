using Microsoft.Extensions.DependencyInjection;
using Pipes.Abstractions;
using Pipes.Caching;

namespace Pipes.DependencyInjection.Caching;

internal class PipeableServiceCache : PipeableType
{
    private ServiceCacheHandler? _cacheHandler;

    public CacheFlags CacheFlags { get; }

    public PipeableServiceCache(Type type, CacheFlags cacheFlags) : base(type)
    {
        CacheFlags = cacheFlags;
    }

    public override object? ConvertInput(object? input)
    {
        return _cacheHandler!.ConvertInput(this, input);
    }

    public override void Execute(IPipe<object, object?> pipe)
    {
        _cacheHandler!.Execute(this, pipe);
    }

    public override Task ExecuteAsync(IPipe<object, object?> pipe, CancellationToken cancellationToken = default)
    {
        return _cacheHandler!.ExecuteAsync(this, pipe, cancellationToken);
    }

    protected override void ActivateType()
    {
        base.ActivateType();

        _cacheHandler = ServiceProvider!.GetService<ServiceCacheHandler>();
    }
}
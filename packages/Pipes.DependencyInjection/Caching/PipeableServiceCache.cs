using Pipes.Abstractions;
using Pipes.Caching;

namespace Pipes.DependencyInjection.Caching;

internal class PipeableServiceCache : IPipeable<object, object>
{
    private readonly CacheFlags _cacheFlags;
    
    public PipeableServiceCache(Type type, CacheFlags cacheFlags)
    {
        _cacheFlags = cacheFlags;
    }

    public object? ConvertInput(object? input)
    {
        throw new NotImplementedException();
    }

    public void Execute(IPipe<object, object?> pipe)
    {
        throw new NotImplementedException();
    }

    public Task ExecuteAsync(IPipe<object, object?> pipe, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
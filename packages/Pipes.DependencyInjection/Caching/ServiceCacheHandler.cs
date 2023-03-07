using Pipes.Abstractions;
using Pipes.Caching;

namespace Pipes.DependencyInjection.Caching;

internal class ServiceCacheHandler : IDisposable
{
    private readonly IDictionary<string, object> _caches = new Dictionary<string, object>();

    public void Dispose()
    {
        _caches.Clear();
    }

    public object? ConvertInput(PipeableServiceCache serviceCache, object? input)
    {
        var cache = VerifyCache(serviceCache);

        var convertInputMethod = cache!.GetType().GetMethod(nameof(ConvertInput))!;
        return convertInputMethod.Invoke(cache, new[] { input });
    }

    public void Execute(PipeableServiceCache serviceCache, IPipe<object, object?> pipe)
    {
        var cache = VerifyCache(serviceCache);

        var executeMethod = cache!.GetType().GetMethod(nameof(Execute));
        executeMethod!.Invoke(cache, new object[] { pipe });
    }

    public async Task ExecuteAsync(PipeableServiceCache serviceCache, IPipe<object, object?> pipe,
        CancellationToken cancellationToken = default)
    {
        var cache = VerifyCache(serviceCache);

        var executeAsyncMethod = cache!.GetType().GetMethod(nameof(ExecuteAsync));
        var task = (Task)executeAsyncMethod!.Invoke(cache, new object[] { pipe, cancellationToken })!;

        await task.ConfigureAwait(false);
    }

    public void Clear(PipeableServiceCache serviceCache)
    {
        var cache = VerifyCache(serviceCache, false);

        if (cache == null) return;
        
        var clearMethod = cache.GetType().GetMethod(nameof(Clear));
        clearMethod!.Invoke(cache, Array.Empty<object>());
    }
    
    private object? VerifyCache(PipeableServiceCache serviceCache, bool createIfNotExists = true)
    {
        var key =
            $"{serviceCache.Type.FullName}<{serviceCache.InputType},{serviceCache.OutputType}>({serviceCache.CacheFlags})";
        if (_caches.ContainsKey(key)) return _caches[key];

        if (!createIfNotExists) return null;

        var cache = TypeUtils.CreateGenericInstance(typeof(PipeableCache<,>), serviceCache.InputType,
            serviceCache.OutputType, serviceCache.Pipeable!, serviceCache.CacheFlags);

        _caches.Add(key, cache);
        return cache;
    }
}
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

        var convertInputMethod = cache.GetType().GetMethod(nameof(ConvertInput))!;
        return convertInputMethod.Invoke(cache, new[] { input });
    }

    public void Execute(PipeableServiceCache serviceCache, IPipe<object, object?> pipe)
    {
        var cache = VerifyCache(serviceCache);
        var genericPipe =
            TypeUtils.CreateGenericInstance(typeof(GenericPipeImplementation<,>), serviceCache.InputType,
                serviceCache.OutputType, pipe);

        var executeMethod = cache.GetType().GetMethod(nameof(Execute));
        executeMethod!.Invoke(cache, new[] { genericPipe });
    }

    public Task ExecuteAsync(PipeableServiceCache serviceCache, IPipe<object, object?> pipe,
        CancellationToken cancellationToken = default)
    {
        var cache = VerifyCache(serviceCache);
        var genericPipe =
            TypeUtils.CreateGenericInstance(typeof(GenericPipeImplementation<,>), serviceCache.InputType,
                serviceCache.OutputType, pipe);

        var executeAsyncMethod = cache.GetType().GetMethod(nameof(ExecuteAsync));
        return (Task)executeAsyncMethod!.Invoke(cache, new[] { genericPipe, cancellationToken })!;
    }

    private object VerifyCache(PipeableServiceCache serviceCache)
    {
        var key =
            $"{serviceCache.Type.FullName}<{serviceCache.InputType},{serviceCache.OutputType}>({serviceCache.CacheFlags})";
        if (_caches.ContainsKey(key)) return _caches[key];

        var cache = TypeUtils.CreateGenericInstance(typeof(PipeableCache<,>), serviceCache.InputType,
            serviceCache.OutputType, serviceCache.Pipeable!, serviceCache.CacheFlags);

        _caches.Add(key, cache);
        return cache;
    }
}
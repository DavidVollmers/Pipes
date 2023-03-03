using Pipes.Abstractions;
using Pipes.Caching;

namespace Pipes.DependencyInjection.Caching;

public static class Cache
{
    private static IPipeable<object, object> CacheService(Type type, CacheFlags cacheFlags)
    {
        var pipeableInterface = type.GetInterface(typeof(IPipeable<,>).Name);
        if (pipeableInterface == null) throw new Exception("Type must be assignable to IPipeable.");
        return new PipeableCache<object, object>(new PipeableType(type, pipeableInterface), cacheFlags);
    }

    public static IPipeable<object, object> Output<TType>()
    {
        return CacheService(typeof(TType), CacheFlags.Output);
    }

    public static IPipeable<object, object> Input<TType>()
    {
        return CacheService(typeof(TType), CacheFlags.Input);
    }

    public static IPipeable<object, object> Everything<TType>()
    {
        return CacheService(typeof(TType), CacheFlags.Input & CacheFlags.Output);
    }
}
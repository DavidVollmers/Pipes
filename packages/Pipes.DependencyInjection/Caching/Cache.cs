﻿using Pipes.Abstractions;
using Pipes.Caching;

namespace Pipes.DependencyInjection.Caching;

public static class Cache
{
    private static IPipeable<object, object> CacheService(Type type, CacheFlags cacheFlags)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        // var pipeableInterface = type.GetInterface(typeof(IPipeable<,>).Name);
        // if (pipeableInterface == null) throw new Exception("Type must be assignable to IPipeable.");
        // return new PipeableType(type, cacheFlags);
        return new PipeableServiceCache(type, cacheFlags);
    }

    public static IPipeable<object, object> Output<T>()
    {
        return CacheService(typeof(T), CacheFlags.Output);
    }

    public static IPipeable<object, object> Input<T>()
    {
        return CacheService(typeof(T), CacheFlags.Input);
    }

    public static IPipeable<object, object> Everything<T>()
    {
        return CacheService(typeof(T), CacheFlags.Input & CacheFlags.Output);
    }
    
    public static IPipeable<TInput, TOutput> Output<TInput, TOutput>(IPipeable<TInput, TOutput> pipeable)
    {
        if (pipeable == null) throw new ArgumentNullException(nameof(pipeable));
        return new PipeableCache<TInput, TOutput>(pipeable, CacheFlags.Output);
    }

    public static IPipeable<TInput, TOutput> Input<TInput, TOutput>(IPipeable<TInput, TOutput> pipeable)
    {
        if (pipeable == null) throw new ArgumentNullException(nameof(pipeable));
        return new PipeableCache<TInput, TOutput>(pipeable, CacheFlags.Input);
    }

    public static IPipeable<TInput, TOutput> Everything<TInput, TOutput>(IPipeable<TInput, TOutput> pipeable)
    {
        if (pipeable == null) throw new ArgumentNullException(nameof(pipeable));
        return new PipeableCache<TInput, TOutput>(pipeable, CacheFlags.Input & CacheFlags.Output);
    }
}
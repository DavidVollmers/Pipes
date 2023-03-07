namespace Pipes.Caching;

public static class Cache
{
    public static IPipeable<object, object> Output<TInput, TOutput>(IPipeable<TInput, TOutput> pipeable)
    {
        if (pipeable == null) throw new ArgumentNullException(nameof(pipeable));
        return new PipeableCache<TInput, TOutput>(pipeable, CacheFlags.Output);
    }

    public static IPipeable<object, object> Input<TInput, TOutput>(IPipeable<TInput, TOutput> pipeable)
    {
        if (pipeable == null) throw new ArgumentNullException(nameof(pipeable));
        return new PipeableCache<TInput, TOutput>(pipeable, CacheFlags.Input);
    }

    public static IPipeable<object, object> Everything<TInput, TOutput>(IPipeable<TInput, TOutput> pipeable)
    {
        if (pipeable == null) throw new ArgumentNullException(nameof(pipeable));
        return new PipeableCache<TInput, TOutput>(pipeable, CacheFlags.Input & CacheFlags.Output);
    }
}
namespace Pipes.Caching;

public static class Cache
{
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
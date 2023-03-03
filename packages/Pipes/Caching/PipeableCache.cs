namespace Pipes.Caching;

internal class PipeableCache<TInput, TOutput> : IPipeable<TInput, TOutput>
{
    private readonly IPipeable<TInput, TOutput> _pipeable;
    private readonly CacheFlags _cacheFlags;

    private bool _inputCached;
    private TInput? _inputCache;
    private bool _outputCached;
    private TOutput? _outputCache;

    public TOutput? OutputCache
    {
        get => _outputCache;
        set
        {
            _outputCache = value;
            _outputCached = true;
        }
    }

    public PipeableCache(IPipeable<TInput, TOutput> pipeable, CacheFlags cacheFlags)
    {
        _pipeable = pipeable;
        _cacheFlags = cacheFlags;
    }

    public TInput? ConvertInput(object? input)
    {
        if (_inputCached) return _inputCache;

        var result = _pipeable.ConvertInput(input);
        if ((_cacheFlags & CacheFlags.Input) == 0) return result;

        _inputCache = result;
        _inputCached = true;
        return result;
    }

    public void Execute(IPipe<TInput, TOutput?> pipe)
    {
        if (_outputCached)
        {
            pipe.Pipe(OutputCache);
            return;
        }

        var cachePipe = new OutputCachePipeImplementation<TInput, TOutput>(this, pipe);
        _pipeable.Execute(cachePipe!);
    }

    public Task ExecuteAsync(IPipe<TInput, TOutput?> pipe, CancellationToken cancellationToken = default)
    {
        if (_outputCached)
        {
            return pipe.PipeAsync(OutputCache, cancellationToken);
        }

        var cachePipe = new OutputCachePipeImplementation<TInput, TOutput>(this, pipe);
        return _pipeable.ExecuteAsync(cachePipe!, cancellationToken);
    }
}
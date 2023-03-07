namespace Pipes.Caching;

internal class PipeableCache<TInput, TOutput> : IPipeable<object, object>, IPipeableCache
{
    private readonly CacheFlags _cacheFlags;
    private readonly IPipeable<TInput, TOutput> _pipeable;
    private TInput? _inputCache;

    private bool _inputCached;
    private TOutput? _outputCache;
    private bool _outputCached;

    public PipeableCache(IPipeable<TInput, TOutput> pipeable, CacheFlags cacheFlags)
    {
        _pipeable = pipeable;
        _cacheFlags = cacheFlags;
    }

    public TOutput? OutputCache
    {
        get => _outputCache;
        set
        {
            _outputCache = value;
            _outputCached = true;
        }
    }

    public object? ConvertInput(object? input)
    {
        if (_inputCached) return _inputCache;

        var result = _pipeable.ConvertInput(input);
        if ((_cacheFlags & CacheFlags.Input) == 0) return result;

        _inputCache = result;
        _inputCached = true;
        return result;
    }

    public void Execute(IPipe<object, object?> pipe)
    {
        if (_outputCached)
        {
            pipe.Pipe(OutputCache);
            return;
        }

        var cachePipe =
            new OutputCachePipeImplementation<TInput, TOutput>(this,
                new GenericPipeImplementation<TInput, TOutput?>(pipe));
        _pipeable.Execute(cachePipe!);
    }

    public async Task ExecuteAsync(IPipe<object, object?> pipe, CancellationToken cancellationToken = default)
    {
        if (_outputCached)
        {
            await pipe.PipeAsync(OutputCache, cancellationToken).ConfigureAwait(false);
            return;
        }

        var cachePipe =
            new OutputCachePipeImplementation<TInput, TOutput>(this,
                new GenericPipeImplementation<TInput, TOutput?>(pipe));
        await _pipeable.ExecuteAsync(cachePipe!, cancellationToken).ConfigureAwait(false);
    }

    public void Clear()
    {
        _outputCache = default;
        _outputCached = false;
        _inputCache = default;
        _inputCached = false;
    }
}
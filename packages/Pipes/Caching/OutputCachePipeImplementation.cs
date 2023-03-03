namespace Pipes.Caching;

internal class OutputCachePipeImplementation<TInput, TOutput> : IPipe<TInput, TOutput>
{
    private readonly PipeableCache<TInput, TOutput> _pipeable;
    private readonly IPipe<TInput, TOutput> _pipe;

    public OutputCachePipeImplementation(PipeableCache<TInput, TOutput> pipeable, IPipe<TInput, TOutput> pipe)
    {
        _pipeable = pipeable;
        _pipe = pipe;
    }

    public TInput? Input => _pipe.Input;
    
    public void Pipe(TOutput? input)
    {
        _pipeable.OutputCache = input;
        _pipe.Pipe(input);
    }

    public Task PipeAsync(TOutput? input, CancellationToken cancellationToken = default)
    {
        _pipeable.OutputCache = input;
        return _pipe.PipeAsync(input, cancellationToken);
    }
}
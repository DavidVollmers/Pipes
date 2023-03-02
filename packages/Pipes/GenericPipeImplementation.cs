namespace Pipes;

internal class GenericPipeImplementation<TInput, TOutput> : IPipe<TInput, TOutput>
{
    private readonly IPipe<object, object> _inner;

    public TInput? Input => (TInput?)_inner.Input;

    public GenericPipeImplementation(IPipe<object, object> inner)
    {
        _inner = inner;
    }

    public void Pipe(TOutput? input)
    {
        _inner.Pipe(input);
    }

    public Task PipeAsync(TOutput? input, CancellationToken cancellationToken = default)
    {
        return _inner.PipeAsync(input, cancellationToken);
    }
}
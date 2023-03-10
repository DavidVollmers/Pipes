namespace Pipes;

internal class GenericPipeImplementation<TInput, TOutput> : IPipe<TInput, TOutput>
{
    private readonly IPipe<object, object> _inner;

    public GenericPipeImplementation(IPipe<object, object> inner)
    {
        _inner = inner;
    }

    public TInput? Input => (TInput?)_inner.Input;

    public void Pipe(TOutput? input)
    {
        _inner.Pipe(input);
    }

    public async Task PipeAsync(TOutput? input, CancellationToken cancellationToken = default)
    {
        await _inner.PipeAsync(input, cancellationToken).ConfigureAwait(false);
    }
}
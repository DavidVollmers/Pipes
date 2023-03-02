namespace Pipes;

internal class GenericPipeable<TInput, TOutput> : IPipeable<object, object>
{
    private readonly IPipeable<TInput, TOutput> _inner;

    public GenericPipeable(IPipeable<TInput, TOutput> inner)
    {
        _inner = inner;
    }

    public object? ConvertInput(object? input)
    {
        return _inner.ConvertInput(input);
    }

    public void Execute(IPipe<object, object?> pipe)
    {
        _inner.Execute(new GenericPipeImplementation<TInput, TOutput?>(pipe));
    }

    public Task ExecuteAsync(IPipe<object, object?> pipe, CancellationToken cancellationToken = default)
    {
        return _inner.ExecuteAsync(new GenericPipeImplementation<TInput, TOutput?>(pipe), cancellationToken);
    }
}
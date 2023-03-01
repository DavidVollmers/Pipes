using Pipes.Abstractions;

namespace Pipes;

internal class GenericPipeable<TInput, TOutput> : Pipeable<object, object>
{
    private readonly Pipeable<TInput, TOutput> _inner;

    public GenericPipeable(Pipeable<TInput, TOutput> inner)
    {
        _inner = inner;
    }

    public override object? ConvertInput(object? input)
    {
        return _inner.ConvertInput(input);
    }

    public override void Execute(IPipe<object, object?> pipe)
    {
        _inner.Execute(new GenericPipeImplementation<TInput, TOutput?>(pipe));
    }

    public override Task ExecuteAsync(IPipe<object, object?> pipe, CancellationToken cancellationToken = default)
    {
        return _inner.ExecuteAsync(new GenericPipeImplementation<TInput, TOutput?>(pipe), cancellationToken);
    }
}
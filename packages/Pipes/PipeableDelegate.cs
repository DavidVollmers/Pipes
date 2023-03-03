using Pipes.Input;

namespace Pipes;

internal class PipeableDelegate<TInput, TOutput> : IPipeable<object, object>
{
    private readonly Func<TInput, TOutput?> _pipeable;

    public PipeableDelegate(Func<TInput, TOutput?> pipeable)
    {
        _pipeable = pipeable;
    }

    public object? ConvertInput(object? input)
    {
        return InputConverter.ConvertInput<TInput>(input);
    }

    public void Execute(IPipe<object, object?> pipe)
    {
        var output = _pipeable((TInput)pipe.Input!);

        pipe.Pipe(output);
    }

    public Task ExecuteAsync(IPipe<object, object?> pipe, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
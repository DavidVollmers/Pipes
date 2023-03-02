using Pipes.Input;

namespace Pipes;

public class AsyncPipeableDelegate<TInput, TOutput> : IPipeable<object, object>
{
    private readonly Func<TInput, Task<TOutput?>> _pipeable;

    public AsyncPipeableDelegate(Func<TInput, Task<TOutput?>> pipeable)
    {
        _pipeable = pipeable;
    }

    public object? ConvertInput(object? input) => InputConverter.ConvertInput<TInput>(input);

    public void Execute(IPipe<object, object?> pipe)
    {
    }

    public async Task ExecuteAsync(IPipe<object, object?> pipe, CancellationToken cancellationToken = default)
    {
        var output = await _pipeable((TInput)pipe.Input!).ConfigureAwait(false);

        await pipe.PipeAsync(output, cancellationToken).ConfigureAwait(false);
    }
}
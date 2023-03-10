namespace Pipes.DependencyInjection.Tests.Pipeables;

public class AsyncServicePipeable : Pipeable
{
    private readonly CounterService _counter;

    public AsyncServicePipeable(CounterService counter)
    {
        _counter = counter;
    }

    public override Task ExecuteAsync(IPipe<object, object?> pipe, CancellationToken cancellationToken = default)
    {
        _counter.Increment();

        return pipe.PipeAsync(_counter.Value, cancellationToken);
    }
}
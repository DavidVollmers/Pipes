namespace Pipes.DependencyInjection.Tests.Pipeables;

public class ServicePipeable : Pipeable
{
    private readonly CounterService _counter;

    public ServicePipeable(CounterService counter)
    {
        _counter = counter;
    }

    public override void Execute(IPipe<object, object?> pipe)
    {
        _counter.Increment();

        pipe.Pipe(_counter.Value);
    }
}
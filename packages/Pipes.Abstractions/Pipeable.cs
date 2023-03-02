namespace Pipes.Abstractions;

public abstract class Pipeable : IPipeable<object, object>
{
    public object ConvertInput(object? input)
    {
        return input ?? throw new PipeInputNullException(nameof(input));
    }

    public virtual void Execute(IPipe<object, object?> pipe)
    {
    }

    public virtual Task ExecuteAsync(IPipe<object, object?> pipe, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}

public abstract class Pipeable<TInput, TOutput> : IPipeable<TInput, TOutput>
{
    public abstract TInput ConvertInput(object? input);

    public virtual void Execute(IPipe<TInput, TOutput?> pipe)
    {
    }

    public virtual Task ExecuteAsync(IPipe<TInput, TOutput?> pipe, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
namespace Pipes.Abstractions;

public abstract class Pipeable : Pipeable<object, object>
{
}

//TODO introduce IPipeable interface
public abstract class Pipeable<TInput, TOutput>
{
    public abstract TInput? ConvertInput(object? input);
    
    public virtual void Execute(IPipe<TInput, TOutput?> pipe)
    {
    }

    public virtual Task ExecuteAsync(IPipe<TInput, TOutput?> pipe, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
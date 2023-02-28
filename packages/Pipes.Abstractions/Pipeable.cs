namespace Pipes.Abstractions;

//TODO
// public abstract class Pipeable : Pipeable<object, object>
// {
// }

public abstract class Pipeable<TInput, TOutput>
{
    public abstract TInput ConvertInput(object input);

    public virtual void Execute(IPipe<TInput, TOutput?> pipe)
    {
        pipe.Pipe(default);
    }

    public virtual Task ExecuteAsync(IPipe<TInput, TOutput?> pipe)
    {
        Execute(pipe);

        return Task.CompletedTask;
    }

    public virtual Task ExecuteAsync(IPipe<TInput, TOutput?> pipe, CancellationToken cancellationToken)
    {
        return ExecuteAsync(pipe);
    }
}
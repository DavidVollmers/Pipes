namespace Pipes.Abstractions;

public abstract class Pipeable
{
    public virtual void Execute(IPipe pipe)
    {
    }

    public virtual Task ExecuteAsync(IPipe pipe)
    {
        Execute(pipe);

        return Task.CompletedTask;
    }

    public virtual Task ExecuteAsync(IPipe pipe, CancellationToken cancellationToken)
    {
        return ExecuteAsync(pipe);
    }
}

public abstract class Pipeable<TInput, TOutput> : Pipeable
{
    public virtual TOutput? Execute(IPipe<TInput, TOutput> pipe)
    {
        base.Execute(pipe);

        return default;
    }

    public virtual Task<TOutput?> ExecuteAsync(IPipe<TInput, TOutput> pipe)
    {
        var result = Execute(pipe);

        return Task.FromResult(result);
    }

    public virtual Task<TOutput?> ExecuteAsync(IPipe<TInput, TOutput> pipe, CancellationToken cancellationToken)
    {
        return ExecuteAsync(pipe);
    }
}
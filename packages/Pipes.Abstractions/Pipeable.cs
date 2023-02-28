namespace Pipes.Abstractions;

public abstract class Pipeable
{
    public virtual void Execute(Pipeline pipeline)
    {
    }

    public virtual Task ExecuteAsync(Pipeline pipeline)
    {
        Execute(pipeline);

        return Task.CompletedTask;
    }

    public virtual Task ExecuteAsync(Pipeline pipeline, CancellationToken cancellationToken)
    {
        return ExecuteAsync(pipeline);
    }
}

public abstract class Pipeable<TInput, TOutput> : Pipeable
{
    public virtual TOutput? Execute(Pipeline<TInput, TOutput> pipeline)
    {
        base.Execute(pipeline);

        return default;
    }

    public virtual Task<TOutput?> ExecuteAsync(Pipeline<TInput, TOutput> pipeline)
    {
        var result = Execute(pipeline);

        return Task.FromResult(result);
    }

    public virtual Task<TOutput?> ExecuteAsync(Pipeline<TInput, TOutput> pipeline, CancellationToken cancellationToken)
    {
        return ExecuteAsync(pipeline);
    }
}
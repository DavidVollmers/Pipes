namespace Pipes.Abstractions;

public abstract class Pipeable<TInput, TOutput>
{
    private bool _used;

    internal void Reset()
    {
        _used = false;
    }

    public abstract TInput? ConvertInput(object? input);

    public virtual void Execute(IPipe<TInput, TOutput?> pipe)
    {
        if (_used) return;
        _used = true;

        ExecuteAsync(pipe).Wait();
    }

    public virtual Task ExecuteAsync(IPipe<TInput, TOutput?> pipe)
    {
        if (_used) return Task.CompletedTask;
        _used = true;

        Execute(pipe);

        return Task.CompletedTask;
    }

    public virtual Task ExecuteAsync(IPipe<TInput, TOutput?> pipe, CancellationToken cancellationToken)
    {
        return ExecuteAsync(pipe);
    }
}
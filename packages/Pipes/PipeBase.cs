namespace Pipes;

internal abstract class PipeBase : IPipe<object, object>
{
    public bool Used { get; private set; }

    public abstract object? Input { get; }

    public abstract void Pipe(object? input);

    public abstract Task PipeAsync(object? input, CancellationToken cancellationToken = default);

    protected void Invalidate()
    {
        if (Used)
            throw new InvalidOperationException(
                "Pipe already used. Make sure to only make one call to either .Pipe() or .PipeAsync() when implementing custom pipeables.");
        Used = true;
    }
}
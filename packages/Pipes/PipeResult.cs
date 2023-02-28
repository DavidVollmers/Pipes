namespace Pipes;

public sealed class PipeResult<TResult>
{
    public TResult? Result { get; internal set; }
}
namespace Pipes;

public sealed class PipeResult<T>
{
    public T? Result { get; internal set; }
}
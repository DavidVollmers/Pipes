namespace Pipes.Abstractions;

public sealed class PipeOutput<TOutput>
{
    public TOutput? Result { get; internal set; }
}
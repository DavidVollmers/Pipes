namespace Pipes;

public abstract class PipeOutput
{
    internal OutputPipe? Pipe { get; set; }

    public object? Output => Pipe?.Output;
}
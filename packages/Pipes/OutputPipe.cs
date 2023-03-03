namespace Pipes;

internal class OutputPipe : PipeBase
{
    private readonly PipeOutput _output;

    public OutputPipe(PipeOutput output, object? input)
    {
        _output = output;

        Input = input;
    }

    public override object? Input { get; }

    public object? Output { get; private set; }

    public override void Pipe(object? input)
    {
        Invalidate();

        _output.Pipe = this;

        Output = input;
    }

    public override Task PipeAsync(object? input, CancellationToken cancellationToken = default)
    {
        Invalidate();

        _output.Pipe = this;

        Output = input;

        return Task.CompletedTask;
    }
}
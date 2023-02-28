using Pipes.Abstractions;

namespace Pipes;

internal class OutputPipe : IPipe<object, object>
{
    private readonly PipeOutput _output;

    public object? Input { get; }
    
    public OutputPipe(PipeOutput output, object? input)
    {
        _output = output;

        Input = input;
    }
    
    public void Pipe(object? input)
    {
        _output.Output = input;
    }

    public Task PipeAsync(object? input, CancellationToken cancellationToken = default)
    {
        _output.Output = input;
        
        return Task.CompletedTask;
    }
}
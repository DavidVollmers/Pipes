namespace Pipes.Abstractions;

public interface IPipe<out TInput, in TOutput>
{
    public TInput? Input { get; }

    void Pipe(TOutput? input);

    Task PipeAsync(TOutput? input, CancellationToken cancellationToken = default);
}
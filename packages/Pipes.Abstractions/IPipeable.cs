namespace Pipes.Abstractions;

public interface IPipeable<TInput, out TOutput>
{
    TInput? ConvertInput(object? input);

    void Execute(IPipe<TInput, TOutput?> pipe);

    Task ExecuteAsync(IPipe<TInput, TOutput?> pipe, CancellationToken cancellationToken = default);
}
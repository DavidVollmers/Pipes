namespace Pipes.Abstractions;

public interface IAsyncPipeable<TInput, TOutput>
{
    Task ExecuteAsync(Pipeline<TInput, TOutput> pipeline);
}
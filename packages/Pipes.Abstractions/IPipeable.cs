namespace Pipes.Abstractions;

public interface IPipeable<TInput, TOutput>
{
    void Execute(Pipeline<TInput, TOutput> pipeline);
}
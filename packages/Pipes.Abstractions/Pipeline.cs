namespace Pipes.Abstractions;

public abstract class Pipeline<TInput, TOutput>
{
    public abstract TInput Input { get; }

    public abstract void Next(TOutput output);
}
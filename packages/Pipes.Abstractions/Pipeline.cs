namespace Pipes.Abstractions;

public abstract class Pipeline
{
}

public abstract class Pipeline<TInput, TOutput> : Pipeline
{
    public abstract TInput Input { get; }
}
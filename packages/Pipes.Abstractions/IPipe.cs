namespace Pipes.Abstractions;

public interface IPipe
{
}

public interface IPipe<out TInput, TOutput> : IPipe
{
    public TInput Input { get; }
}
using Pipes.Abstractions;

namespace Pipes;

internal class InternalPipe : IPipe
{
}

internal class InternalPipe<TInput, TOutput> : IPipe<TInput, TOutput>
{
    public TInput Input { get; private set; }

    public InternalPipe(TInput input)
    {
        Input = input;
    }
}

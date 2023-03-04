namespace Pipes.AspNetCore;

public sealed class PipeBuilder
{
    internal PipeBuilder()
    {
    }

    public PipeBuilder Add<TInput, TOutput>(Pipe<TInput, TOutput> pipe)
    {
        if (pipe == null) throw new ArgumentNullException(nameof(pipe));
        return this;
    }
}
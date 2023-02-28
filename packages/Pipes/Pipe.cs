using Pipes.Abstractions;

namespace Pipes;

public class Pipe
{
    public Pipe Append<TPipeInput, TPipeOutput>(IPipeable<TPipeInput, TPipeOutput> pipeable)
    {
        return this;
    }

    public Pipe Append<TPipeInput, TPipeOutput>(IAsyncPipeable<TPipeInput, TPipeOutput> pipeable)
    {
        return this;
    }

    public void Execute()
    {
        ExecuteAsync().Wait();
    }

    public Task ExecuteAsync()
    {
        return Task.CompletedTask;
    }
}

public class Pipe<TInput, TOutput> : Pipe
{
    public TOutput? Execute(TInput input)
    {
        return default;
    }

    public Task<TOutput?> ExecuteAsync(TInput input)
    {
        return Task.FromResult(default(TOutput));
    }
}
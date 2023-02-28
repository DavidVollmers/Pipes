namespace Pipes;

public class Pipe : PipeBuilder
{
    public void Execute()
    {
        if (Length == 0) return;

        var pipe = Build();
        return pipe.Pipe();
    }

    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        if (Length == 0) return Task.CompletedTask;
        
        var pipe = Build();
        return pipe.PipeAsync(cancellationToken);
    }
}

public class Pipe<TInput, TOutput> : PipeBuilder
{
    public TOutput? Execute(TInput input)
    {
        if (Length == 0) return default;

        var pipe = Build(input);
        return pipe.Pipe<TOutput>(input);
    }

    public Task<TOutput?> ExecuteAsync(TInput input, CancellationToken cancellationToken = default)
    {
        if (Length == 0) return Task.FromResult(default(TOutput));

        var pipe = Build(input);
        return pipe.PipeAsync<TOutput>(input, cancellationToken);
    }
}
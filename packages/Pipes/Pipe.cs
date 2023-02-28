namespace Pipes;

public class Pipe : PipeBuilder
{
    public void Execute()
    {
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
    }
}

public class Pipe<TInput, TOutput> : PipeBuilder
{
    public TOutput? Execute(TInput input)
    {
    }

    public async Task<TOutput?> ExecuteAsync(TInput input, CancellationToken cancellationToken = default)
    {
        
    }
}
namespace Pipes;

public class Pipe : PipelineBuilder
{
    public void Execute()
    {
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
    }
}

public class Pipe<TInput, TOutput> : PipelineBuilder
{
    public TOutput? Execute(TInput input)
    {
    }

    public async Task<TOutput?> ExecuteAsync(TInput input, CancellationToken cancellationToken = default)
    {
    }
}
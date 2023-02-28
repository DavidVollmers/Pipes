namespace Pipes;

public abstract class PipeBase<T> : PipelineBuilder
{
    public PipeResult<T> Execute()
    {
        var task = ExecuteAsync();

        task.Wait();

        return task.Result;
    }

    public Task<PipeResult<T>> ExecuteAsync()
    {
        return Task.FromResult(new PipeResult<T>());
    }
}

public class Pipe : PipeBase<object>
{
}

public class Pipe<TInput, TOutput> : PipeBase<TOutput>
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
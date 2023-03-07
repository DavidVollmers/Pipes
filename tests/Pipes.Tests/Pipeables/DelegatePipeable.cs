namespace Pipes.Tests.Pipeables;

public class DelegatePipeable : DelegatePipeable<object, object>
{
    public DelegatePipeable(ConvertInputDelegate convertInputDelegate, ExecuteDelegate executeDelegate)
        : base(convertInputDelegate, executeDelegate)
    {
    }

    public DelegatePipeable(ConvertInputDelegate convertInputDelegate, ExecuteAsyncDelegate executeAsyncDelegate)
        : base(convertInputDelegate, executeAsyncDelegate)
    {
    }
}

public class DelegatePipeable<TInput, TOutput> : IPipeable<TInput, TOutput>
{
    public delegate TInput? ConvertInputDelegate(object? input);

    public delegate Task ExecuteAsyncDelegate(IPipe<TInput, TOutput?> pipe);

    public delegate void ExecuteDelegate(IPipe<TInput, TOutput?> pipe);

    private readonly ConvertInputDelegate _convertInputDelegate;
    private readonly ExecuteAsyncDelegate? _executeAsyncDelegate;
    private readonly ExecuteDelegate? _executeDelegate;

    public DelegatePipeable(ConvertInputDelegate convertInputDelegate, ExecuteDelegate executeDelegate)
    {
        _convertInputDelegate = convertInputDelegate;
        _executeDelegate = executeDelegate;
    }

    public DelegatePipeable(ConvertInputDelegate convertInputDelegate, ExecuteAsyncDelegate executeAsyncDelegate)
    {
        _convertInputDelegate = convertInputDelegate;
        _executeAsyncDelegate = executeAsyncDelegate;
    }

    public TInput? ConvertInput(object? input)
    {
        return _convertInputDelegate(input);
    }

    public void Execute(IPipe<TInput, TOutput?> pipe)
    {
        _executeDelegate?.Invoke(pipe);
    }

    public Task ExecuteAsync(IPipe<TInput, TOutput?> pipe, CancellationToken cancellationToken = default)
    {
        return _executeAsyncDelegate?.Invoke(pipe) ?? Task.CompletedTask;
    }
}
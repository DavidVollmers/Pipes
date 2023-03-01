using Pipes.Abstractions;

namespace Pipes.Tests.Pipeables;

public class PipeableDelegate : PipeableDelegate<object, object>
{
    public PipeableDelegate(ConvertInputDelegate convertInputDelegate, ExecuteDelegate executeDelegate)
        : base(convertInputDelegate, executeDelegate)
    {
    }

    public PipeableDelegate(ConvertInputDelegate convertInputDelegate, ExecuteAsyncDelegate executeAsyncDelegate)
        : base(convertInputDelegate, executeAsyncDelegate)
    {
    }
}

public class PipeableDelegate<TInput, TOutput> : Pipeable<TInput, TOutput>
{
    public delegate TInput? ConvertInputDelegate(object? input);

    public delegate void ExecuteDelegate(IPipe<TInput, TOutput?> pipe);

    public delegate Task ExecuteAsyncDelegate(IPipe<TInput, TOutput?> pipe);

    private readonly ConvertInputDelegate _convertInputDelegate;
    private readonly ExecuteAsyncDelegate? _executeAsyncDelegate;
    private readonly ExecuteDelegate? _executeDelegate;

    public PipeableDelegate(ConvertInputDelegate convertInputDelegate, ExecuteDelegate executeDelegate)
    {
        _convertInputDelegate = convertInputDelegate;
        _executeDelegate = executeDelegate;
    }

    public PipeableDelegate(ConvertInputDelegate convertInputDelegate, ExecuteAsyncDelegate executeAsyncDelegate)
    {
        _convertInputDelegate = convertInputDelegate;
        _executeAsyncDelegate = executeAsyncDelegate;
    }

    public override TInput? ConvertInput(object? input) => _convertInputDelegate(input);

    public override void Execute(IPipe<TInput, TOutput?> pipe) => _executeDelegate?.Invoke(pipe);

    public override Task ExecuteAsync(IPipe<TInput, TOutput?> pipe, CancellationToken cancellationToken = default)
        => _executeAsyncDelegate?.Invoke(pipe) ?? Task.CompletedTask;
}
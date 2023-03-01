using Pipes.Abstractions;

namespace Pipes;

internal sealed class PipeImplementation : PipeBase
{
    private const string PipeNotExecutedProperlyException =
        "Pipe was not executed properly. Make sure to either call .Pipe() or .PipeAsync() when implementing custom pipeables.";

    private readonly PipeOutput _output;
    private readonly IPipeable<object, object>[] _pipeables;
    private readonly int _nextPipeLocation;
    private readonly CancellationToken _cancellationToken;

    public override object? Input { get; }

    public PipeImplementation(PipeOutput output, IEnumerable<IPipeable<object, object>> pipeables,
        int pipeLocation, object? input, CancellationToken cancellationToken)
    {
        _output = output;
        _pipeables = pipeables.ToArray();
        _nextPipeLocation = pipeLocation;
        _cancellationToken = cancellationToken;

        Input = input;
    }

    public override void Pipe(object? input)
    {
        Invalidate();

        var next = GetNextPipe(input, _cancellationToken);

        var pipeable = _pipeables[_nextPipeLocation];

        pipeable.Execute(next!);

        if (!next.Used) pipeable.ExecuteAsync(next!, _cancellationToken).GetAwaiter().GetResult();

        if (!next.Used) throw new InvalidOperationException(PipeNotExecutedProperlyException);
    }

    public override async Task PipeAsync(object? input, CancellationToken cancellationToken = default)
    {
        Invalidate();

        var next = GetNextPipe(input, cancellationToken);

        var pipeable = _pipeables[_nextPipeLocation];

        await pipeable.ExecuteAsync(next!, cancellationToken).ConfigureAwait(false);

        // ReSharper disable once MethodHasAsyncOverloadWithCancellation
        if (!next.Used) pipeable.Execute(next!);

        if (!next.Used) throw new InvalidOperationException(PipeNotExecutedProperlyException);
    }

    private PipeBase GetNextPipe(object? input, CancellationToken cancellationToken)
    {
        var convertedInput = _pipeables[_nextPipeLocation].ConvertInput(input);

        if (_nextPipeLocation == _pipeables.Length - 1) return new OutputPipe(_output, convertedInput);

        return new PipeImplementation(_output, _pipeables, _nextPipeLocation + 1, convertedInput, cancellationToken);
    }
}
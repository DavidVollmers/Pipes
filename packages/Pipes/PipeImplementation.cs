using Pipes.Abstractions;

namespace Pipes;

internal sealed class PipeImplementation : IPipe<object, object>
{
    public const string PipeAlreadyUsedException =
        "Pipe already used. Make sure to only make one call to either .Pipe() or .PipeAsync() when implementing custom pipeables.";

    public const string PipeNotExecutedProperlyException =
        "Pipe was not executed properly. Make sure to either call .Pipe() or .PipeAsync() when implementing custom pipeables.";

    private readonly PipeOutput _output;
    private readonly Pipeable<object, object>[] _pipeables;
    private readonly int _nextPipeLocation;

    public bool Used { get; set; }

    public object Input { get; }

    public PipeImplementation(PipeOutput output, IEnumerable<Pipeable<object, object>> pipeables,
        int pipeLocation, object? input)
    {
        _output = output;
        _pipeables = pipeables.ToArray();
        _nextPipeLocation = pipeLocation;

        Input = input!;
    }

    public void Pipe(object? input)
    {
        if (Used) throw new InvalidOperationException(PipeAlreadyUsedException);
        Used = true;

        var next = GetNextPipe(input);

        _pipeables[_nextPipeLocation].Execute(next!);

        if (!next.Used) throw new InvalidOperationException(PipeNotExecutedProperlyException);
    }

    public async Task PipeAsync(object? input, CancellationToken cancellationToken = default)
    {
        if (Used) throw new InvalidOperationException(PipeAlreadyUsedException);
        Used = true;

        var next = GetNextPipe(input);

        await _pipeables[_nextPipeLocation].ExecuteAsync(next!, cancellationToken).ConfigureAwait(false);

        if (!next.Used) throw new InvalidOperationException(PipeNotExecutedProperlyException);
    }

    private IPipe<object, object> GetNextPipe(object? input)
    {
        if (_nextPipeLocation >= _pipeables.Length) throw new InvalidOperationException("Nothing to pipe through.");

        var convertedInput = _pipeables[_nextPipeLocation].ConvertInput(input);

        _pipeables[_nextPipeLocation].Reset();

        if (_nextPipeLocation == _pipeables.Length - 1) return new OutputPipe(_output, convertedInput);

        return new PipeImplementation(_output, _pipeables, _nextPipeLocation + 1, convertedInput);
    }
}
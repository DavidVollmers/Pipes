using Pipes.Abstractions;

namespace Pipes;

internal sealed class PipeImplementation : IPipe<object, object>
{
    private readonly PipeOutput _output;
    private readonly Pipeable<object, object>[] _pipeables;
    private readonly int _nextPipeLocation;

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
        var next = GetNextPipe(input);

        _pipeables[_nextPipeLocation].Execute(next!);
    }

    public async Task PipeAsync(object? input, CancellationToken cancellationToken = default)
    {
        var next = GetNextPipe(input);

        await _pipeables[_nextPipeLocation].ExecuteAsync(next!, cancellationToken).ConfigureAwait(false);
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
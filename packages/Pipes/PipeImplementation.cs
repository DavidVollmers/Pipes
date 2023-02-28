using Pipes.Abstractions;

namespace Pipes;

internal sealed class PipeImplementation : IPipe<object, object>
{
    private readonly Pipeable<object, object>[] _pipe;
    private readonly int _currentPipeLocation;

    public object Input { get; }

    public PipeImplementation(IEnumerable<Pipeable<object, object>> pipe, int pipeLocation, object input)
    {
        _pipe = pipe.ToArray();
        _currentPipeLocation = pipeLocation;

        Input = input;
    }

    public void Pipe(object? input)
    {
        throw new MissingPipeException();
    }

    public async Task PipeAsync(object? input, CancellationToken cancellationToken = default)
    {
        if (_currentPipeLocation >= _pipe.Length) throw new MissingPipeException();

        var pipeable = _pipe[_currentPipeLocation];

        var pipe = new PipeImplementation(_pipe, _currentPipeLocation + 1, pipeable.ConvertInput(Input));

        await pipeable.ExecuteAsync(pipe!, cancellationToken).ConfigureAwait(false);
    }
}
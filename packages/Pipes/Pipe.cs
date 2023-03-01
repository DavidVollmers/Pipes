using System.Collections;
using Pipes.Abstractions;

namespace Pipes;

public class Pipe : Pipe<object, object>
{
    public void Execute()
    {
        Execute(null);
    }

    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(null, cancellationToken);
    }
}

public class Pipe<TInput, TOutput> : PipeOutput, IEnumerable<Pipeable<object, object>>
{
    private readonly IList<Pipeable<object, object>> _pipeables = new List<Pipeable<object, object>>();

    public new TOutput? Output => (TOutput?)base.Output;

    public void Reset()
    {
        Pipe = null;
    }

    public TOutput? Execute(TInput? input)
    {
        if (Output != null) return Output;

        var pipe = Build(default);
        if (pipe == null) return default;

        pipe.Pipe(input);

        return VerifyOutput();
    }

    public async Task<TOutput?> ExecuteAsync(TInput? input, CancellationToken cancellationToken = default)
    {
        if (Output != null) return Output;

        var pipe = Build(cancellationToken);
        if (pipe == null) return default;

        await pipe.PipeAsync(input, cancellationToken).ConfigureAwait(false);

        return VerifyOutput();
    }

    private TOutput? VerifyOutput()
    {
        if (Pipe == null) throw new InvalidOperationException(PipeImplementation.PipeNotExecutedProperlyException);

        return Output;
    }

    private IPipe<object, object>? Build(CancellationToken cancellationToken)
    {
        return _pipeables.Count == 0 ? null : new PipeImplementation(this, _pipeables, 0, null, cancellationToken);
    }

    public Pipe<TInput, TOutput> Add(Pipeable<object, object> pipeable)
    {
        if (pipeable == null) throw new ArgumentNullException(nameof(pipeable));
        _pipeables.Add(pipeable);
        return this;
    }

    public IEnumerator<Pipeable<object, object>> GetEnumerator()
    {
        return _pipeables.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
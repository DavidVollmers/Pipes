using System.Collections;
using Pipes.Caching;
using Pipes.Input;

namespace Pipes;

public class Pipe : Pipe<object, object>
{
    public void Execute()
    {
        Execute(PipeInput.Empty);
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        await ExecuteAsync(PipeInput.Empty, cancellationToken).ConfigureAwait(false);
    }
}

public class Pipe<TOutput> : Pipe<object, TOutput>
{
    public TOutput? Execute()
    {
        return Execute(PipeInput.Empty);
    }

    public async Task<TOutput?> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(PipeInput.Empty, cancellationToken).ConfigureAwait(false);
    }
}

public class Pipe<TInput, TOutput> : PipeOutput, IEnumerable<IPipeable<object, object>>, IDisposable
{
    private readonly IList<IPipeable<object, object>> _pipeables = new List<IPipeable<object, object>>();

    public new TOutput? Output { get; private set; }

    public void Dispose()
    {
        Reset();

        GC.SuppressFinalize(this);
    }

    public IEnumerator<IPipeable<object, object>> GetEnumerator()
    {
        return _pipeables.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public virtual void Reset()
    {
        foreach (var pipeable in _pipeables)
        {
            if (pipeable is not IPipeableCache pipeableCache) continue;

            pipeableCache.Clear();
        }

        ResetOutput();
    }

    private void ResetOutput()
    {
        Pipe = null;
        Output = default;
    }

    public TOutput? Execute(TInput? input)
    {
        ResetOutput();

        var pipe = Build(default);
        if (pipe == null) return default;

        pipe.Pipe(input);

        return VerifyOutput();
    }

    public async Task<TOutput?> ExecuteAsync(TInput? input, CancellationToken cancellationToken = default)
    {
        ResetOutput();

        var pipe = Build(cancellationToken);
        if (pipe == null) return default;

        await pipe.PipeAsync(input, cancellationToken).ConfigureAwait(false);

        return VerifyOutput();
    }

    private TOutput? VerifyOutput()
    {
        if (Pipe!.Output == null) return Output = default;

        //TODO support output conversion
        if (Pipe.Output is not TOutput output)
            throw new InvalidOperationException(
                $"Output of type \"{Pipe.Output.GetType().FullName}\" is not supported. Expected output type \"{typeof(TOutput).FullName}\".");

        return Output = output;
    }

    private IPipe<object, object>? Build(CancellationToken cancellationToken)
    {
        return _pipeables.Count == 0 ? null : new PipeImplementation(this, _pipeables, 0, null, cancellationToken);
    }

    public virtual Pipe<TInput, TOutput> Add(IPipeable<object, object> pipeable)
    {
        if (pipeable == null) throw new ArgumentNullException(nameof(pipeable));
        _pipeables.Add(pipeable);
        return this;
    }
}
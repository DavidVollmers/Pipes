using System.Collections;
using Pipes.Abstractions;

namespace Pipes;

public abstract class PipelineBuilder : IEnumerable<Pipeable>
{
    private readonly IList<Pipeable> _pipeables = new List<Pipeable>();

    public PipelineBuilder Add(Pipeable pipeable)
    {
        if (pipeable == null) throw new ArgumentNullException(nameof(pipeable));
        _pipeables.Add(pipeable);
        return this;
    }

    public IEnumerator<Pipeable> GetEnumerator()
    {
        return _pipeables.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
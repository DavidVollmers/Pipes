using System.Collections;
using Pipes.Abstractions;

namespace Pipes;

public abstract class PipeBuilder : IEnumerable<Pipeable>
{
    private readonly IList<Pipeable> _pipeables = new List<Pipeable>();

    public int Length => _pipeables.Count;

    public IPipe? Build(object? input = null)
    {
        
    }

    public PipeBuilder Add(Pipeable pipeable)
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
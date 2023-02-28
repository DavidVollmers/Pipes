using System.Collections;
using Pipes.Abstractions;

namespace Pipes;

public abstract class PipeBuilder : IEnumerable<Pipeable<object, object>>
{
    private readonly IList<Pipeable<object, object>> _pipeables = new List<Pipeable<object, object>>();

    protected IPipe<object, object>? Build(object input)
    {
        if (_pipeables.Count == 0) return null;
        return new PipeImplementation(_pipeables, 0, input);
    }

    public PipeBuilder Add(Pipeable<object, object> pipeable)
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
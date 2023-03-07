using System.Collections;

namespace Pipes.Input;

public class TypeMap<T> : IEnumerable<TypeMapping<object, T>>
{
    private readonly IList<TypeMapping<object, T>> _map = new List<TypeMapping<object, T>>();

    public IEnumerator<TypeMapping<object, T>> GetEnumerator()
    {
        return _map.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public TypeMap<T> Add(TypeMapping<object, T> mapping)
    {
        _map.Add(mapping);
        return this;
    }
}
namespace Pipes.Input;

public static class TypeMapExtensions
{
    public static TypeMap<T1> Add<T1, T2>(this TypeMap<T1> typeMap, TypeMapping<T2, T1> mapping)
    {
        if (typeMap == null) throw new ArgumentNullException(nameof(typeMap));
        typeMap.Add(new TypeMapping<object, T1>(o => mapping.Mapper((T2)o)));
        return typeMap;
    }

    public static TypeMap<T1> Add<T1, T2>(this TypeMap<T1> typeMap, Func<T2, T1> mapper)
    {
        if (typeMap == null) throw new ArgumentNullException(nameof(typeMap));
        typeMap.Add(new TypeMapping<object, T1>(o => mapper((T2)o)));
        return typeMap;
    }
}
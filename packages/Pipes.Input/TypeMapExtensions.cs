namespace Pipes.Input;

public static class TypeMapExtensions
{
    public static TypeMap<T1> Add<T1, T2>(this TypeMap<T1> typeMap, TypeMapping<T2, T1> mapping)
    {
        if (typeMap == null) throw new ArgumentNullException(nameof(typeMap));
        typeMap.Add(new TypeMapping<object, T1>(mapping.Type, o => mapping.Mapper((T2)o)));
        return typeMap;
    }

    public static TypeMap<T1> Add<T1, T2>(this TypeMap<T1> typeMap, Func<T2, T1> mapper)
    {
        if (typeMap == null) throw new ArgumentNullException(nameof(typeMap));
        if (mapper == null) throw new ArgumentNullException(nameof(mapper));
        typeMap.Add(new TypeMapping<object, T1>(typeof(T2), o => mapper((T2)o)));
        return typeMap;
    }
}
﻿namespace Pipes.Input;

public readonly struct TypeMapping<T1, T2>
{
    public Type Type { get; }
    
    public Func<T1, T2> Mapper { get; }

    public TypeMapping(Func<T1, T2> mapper)
    {
        Type = typeof(T1);
        Mapper = mapper;
    }
}
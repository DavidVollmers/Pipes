﻿using Pipes.Abstractions;

namespace Pipes.Input;

public static class InputConverter
{
    public static bool TryConvertInput<T>(object? input, out T? convertedInput)
    {
        convertedInput = default;

        if (input is not T i) return false;
        
        convertedInput = i;
        return true;

    }

    public static T ConvertInput<T>(object? input)
    {
        if (input == null) throw new PipeInputNullException(nameof(input));
        if (TryConvertInput(input, out T? convertedInput)) return convertedInput!;
        throw new PipeInputNotSupportedException(input.GetType(), typeof(T));
    }

    public static T ConvertInputByTypeMap<T>(object? input, TypeMap<T> typeMap)
    {
        if (typeMap == null) throw new ArgumentNullException(nameof(typeMap));
        
        if (input == null) throw new PipeInputNullException(nameof(input));

        foreach (var mapping in typeMap)
        {
            if (TryConvertInput(mapping.Type, input, out var convertedInput))
                return mapping.Mapper(convertedInput!);
        }

        throw new PipeInputNotSupportedException(input.GetType(), typeof(T));
    }

    private static bool TryConvertInput(Type type, object? input, out object? convertedInput)
    {
        convertedInput = type.IsValueType ? Activator.CreateInstance(type) : null;

        if (input?.GetType() != type) return false;

        convertedInput = input;
        return true;
    }
}
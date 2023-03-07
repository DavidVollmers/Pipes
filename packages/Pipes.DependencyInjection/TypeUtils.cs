namespace Pipes.DependencyInjection;

internal static class TypeUtils
{
    public static object CreateGenericInstance(Type type, Type inputType, Type outputType, params object[] args)
    {
        var genericPipeType = type.MakeGenericType(inputType, outputType);
        return Activator.CreateInstance(genericPipeType, args)!;
    }
}
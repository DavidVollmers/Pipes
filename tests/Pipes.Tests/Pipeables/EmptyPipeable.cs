using Pipes.Abstractions;

namespace Pipes.Tests.Pipeables;

public class EmptyPipeable : Pipeable<object, object>
{
    public override object? ConvertInput(object? input)
    {
        return input;
    }
}
namespace Pipes.Abstractions;

public class PipeInputNotSupportedException : Exception
{
    public Type InputType { get; }

    public Type TargetType { get; }
    
    public PipeInputNotSupportedException(Type inputType, Type targetType) : base(
        $"Input of type \"{inputType.FullName}\" is not supported. Expected input type \"{targetType.FullName}\" or a valid conversion.")
    {
        InputType = inputType;
        TargetType = targetType;
    }
}
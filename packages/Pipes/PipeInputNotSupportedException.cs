namespace Pipes;

public class PipeInputNotSupportedException : Exception
{
    public PipeInputNotSupportedException(Type inputType, Type targetType) : base(
        $"Input of type \"{inputType.FullName}\" is not supported. Expected input type \"{targetType.FullName}\" or a valid conversion.")
    {
    }
}
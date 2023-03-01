namespace Pipes.Tests.Exceptions;

public class PipeInputNotSupportedExceptionTests
{
    [Fact]
    public void Test_Message()
    {
        var inputType = typeof(int);
        var targetType = typeof(string);
        var expectedMessage =
            $"Input of type \"{inputType.FullName}\" is not supported. Expected input type \"{targetType.FullName}\" or a valid conversion.";
        
        var exception = new PipeInputNotSupportedException(inputType, targetType);
        Assert.Equal(expectedMessage, exception.Message);
        Assert.Equal(inputType, exception.InputType);
        Assert.Equal(targetType, exception.TargetType);
    }
}
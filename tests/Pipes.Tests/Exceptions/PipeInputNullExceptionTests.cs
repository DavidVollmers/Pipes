namespace Pipes.Tests.Exceptions;

public class PipeInputNullExceptionTests
{
    [Fact]
    public void Test_Message()
    {
        var inputName = Guid.NewGuid().ToString();
        var expectedMessage = new InvalidPipeInputException(inputName, "null").Message;

        var exception = new PipeInputNullException(inputName);
        Assert.Equal(expectedMessage, exception.Message);
        Assert.Equal(inputName, exception.InputName);
    }
}
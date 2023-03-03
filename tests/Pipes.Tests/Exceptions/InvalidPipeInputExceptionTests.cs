namespace Pipes.Tests.Exceptions;

public class InvalidPipeInputExceptionTests
{
    [Fact]
    public void Test_Message()
    {
        var inputName = Guid.NewGuid().ToString();
        var inputValue = 32857;
        var expectedMessage = $"Invalid pipe input for \"{inputName}\": {inputValue}";

        var exception = new InvalidPipeInputException(inputName, inputValue);
        Assert.Equal(expectedMessage, exception.Message);
        Assert.Equal(inputName, exception.InputName);
        Assert.Equal(inputValue, exception.InputValue);
    }
}
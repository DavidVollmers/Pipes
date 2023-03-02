namespace Pipes.Input.Tests;

public class InputConverterTests
{
    [Fact]
    public void Test_TryConvertInput_Null()
    {
        var result = InputConverter.TryConvertInput(null, out dynamic? _);
        Assert.False(result);
    }

    [Fact]
    public void Test_TryConvertInput_TypeMatch()
    {
        var input = 21528;

        var result = InputConverter.TryConvertInput(input, out int _);
        Assert.True(result);
    }

    [Fact]
    public void Test_TryConvertInput_NoMatch()
    {
        var input = 1245125;

        var result = InputConverter.TryConvertInput(input, out string _);
        Assert.False(result);
    }
}
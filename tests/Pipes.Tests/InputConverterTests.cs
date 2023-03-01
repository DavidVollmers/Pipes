namespace Pipes.Tests;

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
    public void Test_TryConvertInput_EnumerableNotAllowed()
    {
        var input = new[] { 21528 };

        var result = InputConverter.TryConvertInput(input, out int _, false);
        Assert.False(result);
    }

    [Fact]
    public void Test_TryConvertInput_SingleEnumerable()
    {
        var input = new[] { 21528 };

        var result = InputConverter.TryConvertInput(input, out int _);
        Assert.True(result);
    }

    [Fact]
    public void Test_TryConvertInput_NotSingleEnumerable()
    {
        var input = new[] { 21528, 21253 };

        var exception =
            Assert.Throws<InvalidOperationException>(
                () => InputConverter.TryConvertInput(input, out int _));
        Assert.Equal("Sequence contains more than one element", exception.Message);
    }

    [Fact]
    public void Test_TryConvertInput_SingleNullEnumerable()
    {
        var input = new string[] { null };

        var result = InputConverter.TryConvertInput(input, out string _);
        Assert.False(result);
    }

    [Fact]
    public void Test_TryConvertInput_NoMatch()
    {
        var input = 1245125;

        var result = InputConverter.TryConvertInput(input, out string _);
        Assert.False(result);
    }
}
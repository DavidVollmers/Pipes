namespace Pipes.Input.Tests;

public class InputConverterTests
{
    [Fact]
    public void Test_TryConvertInput_Null()
    {
        var result = InputConverter.TryConvertInput(null, out object? _);
        Assert.False(result);
    }

    [Fact]
    public void Test_TryConvertInput_Match()
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

    [Fact]
    public void Test_ConvertInput_Null()
    {
        var exception = Assert.Throws<PipeInputNullException>(() => InputConverter.ConvertInput<object>(null));
        Assert.Equal("input", exception.InputName);
    }

    [Fact]
    public void Test_ConvertInput_Match()
    {
        var input = 1245125;

        var result = InputConverter.ConvertInput<int>(input);
        Assert.Equal(input, result);
    }

    [Fact]
    public void Test_ConvertInput_NoMatch()
    {
        var input = 1245125;

        var exception = Assert.Throws<PipeInputNotSupportedException>(() => InputConverter.ConvertInput<string>(input));
        Assert.Equal(input.GetType(), exception.InputType);
        Assert.Equal(typeof(string), exception.TargetType);
    }

    [Fact]
    public void Test_ConvertInputByTypeMap_TypeMapIsNull()
    {
        var exception =
            Assert.Throws<ArgumentNullException>(() => InputConverter.ConvertInputByTypeMap<object>(null, null!));
        Assert.Equal("typeMap", exception.ParamName);
    }

    [Fact]
    public void Test_ConvertInputByTypeMap_Null()
    {
        var typeMap = new TypeMap<object>();

        var exception =
            Assert.Throws<PipeInputNullException>(() => InputConverter.ConvertInputByTypeMap(null, typeMap));
        Assert.Equal("input", exception.InputName);
    }

    [Fact]
    public void Test_ConvertInputByTypeMap_Match()
    {
        var expectedConversion = 4;
        var input = expectedConversion.ToString();
        var typeMap = new TypeMap<int>
        {
            (string s) => int.Parse(s)
        };

        var result = InputConverter.ConvertInputByTypeMap(input, typeMap);
        Assert.Equal(expectedConversion, result);
    }

    [Fact]
    public void Test_ConvertInputByTypeMap_MatchSecond()
    {
        var expectedConversion = 4;
        var input = expectedConversion.ToString();
        var typeMap = new TypeMap<int>
        {
            (DateTime dt) => (int)dt.Ticks,
            (string s) => int.Parse(s)
        };

        var result = InputConverter.ConvertInputByTypeMap(input, typeMap);
        Assert.Equal(expectedConversion, result);
    }

    [Fact]
    public void Test_ConvertInputByTypeMap_NoMatch()
    {
        var input = 4.ToString();
        var typeMap = new TypeMap<int>
        {
            (DateTime dt) => (int)dt.Ticks
        };

        var exception =
            Assert.Throws<PipeInputNotSupportedException>(() => InputConverter.ConvertInputByTypeMap(input, typeMap));
        Assert.Equal(input.GetType(), exception.InputType);
        Assert.Equal(typeof(int), exception.TargetType);
    }
}
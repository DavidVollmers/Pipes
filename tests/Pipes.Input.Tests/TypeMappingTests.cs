namespace Pipes.Input.Tests;

public class TypeMappingTests
{
    [Fact]
    public void Test_Constructor_MapperIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new TypeMapping<object, object>(null!));
        Assert.Equal("mapper", exception.ParamName);
    }

    [Fact]
    public void Test_Constructor()
    {
        // ReSharper disable once ConvertToLocalFunction
        var mapper = (string s) => int.Parse(s);

        var typeMapping = new TypeMapping<string, int>(mapper);
        Assert.Equal(typeof(string), typeMapping.Type);
        Assert.Equal(mapper, typeMapping.Mapper);
    }
}
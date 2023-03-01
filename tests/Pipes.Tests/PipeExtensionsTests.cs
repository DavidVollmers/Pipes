namespace Pipes.Tests;

public class PipeExtensionsTests
{
    [Fact]
    public void Test_Add_PipeIsNull()
    {
        var exception =
            Assert.Throws<ArgumentNullException>(() => PipeExtensions.Add<object, object, object, object>(null, null));
        Assert.Equal("pipe", exception.ParamName);
    }

    [Fact]
    public void Test_Add_PipeableIsNull()
    {
        var pipe = new Pipe();
        
        var exception =
            Assert.Throws<ArgumentNullException>(() => PipeExtensions.Add<object, object, object, object>(pipe, null));
        Assert.Equal("pipeable", exception.ParamName);
    }
}
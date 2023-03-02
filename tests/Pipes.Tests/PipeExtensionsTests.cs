namespace Pipes.Tests;

public class PipeExtensionsTests
{
    [Fact]
    public void Test_Add_PipeIsNull()
    {
        IPipeable<object, object> pipeable = null!;
        
        var exception =
            Assert.Throws<ArgumentNullException>(() => PipeExtensions.Add<object, object, object, object>(null!, pipeable));
        Assert.Equal("pipe", exception.ParamName);
    }

    [Fact]
    public void Test_Add_PipeableIsNull()
    {
        var pipe = new Pipe();
        IPipeable<object, object> pipeable = null!;
        
        var exception =
            Assert.Throws<ArgumentNullException>(() => PipeExtensions.Add(pipe, pipeable));
        Assert.Equal("pipeable", exception.ParamName);
    }
}
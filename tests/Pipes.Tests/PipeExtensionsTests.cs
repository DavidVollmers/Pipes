using Pipes.Tests.Pipeables;

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

    [Fact]
    public void Test_Add_Pipeable()
    {
        var pipe = new Pipe();
        var pipeable = new EmptyPipeable();

        PipeExtensions.Add(pipe, pipeable);
    }
    
    [Fact]
    public void Test_Add_Delegate_PipeIsNull()
    {
        Func<object, object> pipeable = null!;
        
        var exception =
            Assert.Throws<ArgumentNullException>(() => PipeExtensions.Add<object, object, object, object>(null!, pipeable));
        Assert.Equal("pipe", exception.ParamName);
    }

    [Fact]
    public void Test_Add_Delegate_PipeableIsNull()
    {
        var pipe = new Pipe();
        Func<object, object> pipeable = null!;
        
        var exception =
            // ReSharper disable once InvokeAsExtensionMethod
            Assert.Throws<ArgumentNullException>(() => PipeExtensions.Add(pipe, pipeable));
        Assert.Equal("pipeable", exception.ParamName);
    }

    [Fact]
    public void Test_Add_Delegate_Pipeable()
    {
        var pipe = new Pipe();
        // ReSharper disable once ConvertToLocalFunction
        Func<object, object> pipeable = _ => _;

        // ReSharper disable once InvokeAsExtensionMethod
        PipeExtensions.Add(pipe, pipeable);
    }
    
    [Fact]
    public void Test_Add_AsyncDelegate_PipeIsNull()
    {
        Func<object, Task<object>> pipeable = null!;
        
        var exception =
            Assert.Throws<ArgumentNullException>(() => PipeExtensions.Add<object, object, object, object>(null!, pipeable!));
        Assert.Equal("pipe", exception.ParamName);
    }

    [Fact]
    public void Test_Add_AsyncDelegate_PipeableIsNull()
    {
        var pipe = new Pipe();
        Func<object, Task<object>> pipeable = null!;
        
        var exception =
            // ReSharper disable once InvokeAsExtensionMethod
            Assert.Throws<ArgumentNullException>(() => PipeExtensions.Add(pipe, pipeable!));
        Assert.Equal("pipeable", exception.ParamName);
    }

    [Fact]
    public void Test_Add_AsyncDelegate_Pipeable()
    {
        var pipe = new Pipe();
        // ReSharper disable once ConvertToLocalFunction
#pragma warning disable CS1998
        Func<object, Task<object>> pipeable = async _ => _;
#pragma warning restore CS1998

        // ReSharper disable once InvokeAsExtensionMethod
        PipeExtensions.Add(pipe, pipeable!);
    }
}
using Pipes.Caching;
using Pipes.Tests.Pipeables;

namespace Pipes.Tests.Caching;

public class CacheTests
{
    [Fact]
    public void Test_Output_PipeableIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => Cache.Output<object, object>(null!));
        Assert.Equal("pipeable", exception.ParamName);
    }
    
    [Fact]
    public void Test_Output()
    {
        var pipe = new Pipe
        {
            Cache.Output(new EmptyPipeable()),
            Cache.Output(new DelegatePipeable<int, string>(null!, null!))
        };
        
        Assert.Equal(2, pipe.Count());
    }
    
    [Fact]
    public void Test_Input_PipeableIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => Cache.Input<object, object>(null!));
        Assert.Equal("pipeable", exception.ParamName);
    }
    
    [Fact]
    public void Test_Input()
    {
        var pipe = new Pipe
        {
            Cache.Input(new EmptyPipeable()),
            Cache.Input(new DelegatePipeable<int, string>(null!, null!))
        };
        
        Assert.Equal(2, pipe.Count());
    }
    
    [Fact]
    public void Test_Everything_PipeableIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => Cache.Everything<object, object>(null!));
        Assert.Equal("pipeable", exception.ParamName);
    }
    
    [Fact]
    public void Test_Everything()
    {
        var pipe = new Pipe
        {
            Cache.Everything(new EmptyPipeable()),
            Cache.Everything(new DelegatePipeable<int, string>(null!, null!))
        };
        
        Assert.Equal(2, pipe.Count());
    }
}
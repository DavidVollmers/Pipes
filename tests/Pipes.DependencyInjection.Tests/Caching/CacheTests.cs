using Pipes.DependencyInjection.Caching;
using Pipes.DependencyInjection.Tests.Pipeables;

namespace Pipes.DependencyInjection.Tests.Caching;

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
            Cache.Output(new ServicePipeable(null!))
        };
        
        Assert.Single(pipe);
    }
    
    [Fact]
    public void Test_Output_Service()
    {
        var pipe = new Pipe
        {
            Cache.Output<ServicePipeable>()
        };
        
        Assert.Single(pipe);
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
            Cache.Input(new ServicePipeable(null!))
        };
        
        Assert.Single(pipe);
    }
    
    [Fact]
    public void Test_Input_Service()
    {
        var pipe = new Pipe
        {
            Cache.Input<ServicePipeable>()
        };
        
        Assert.Single(pipe);
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
            Cache.Everything(new ServicePipeable(null!))
        };
        
        Assert.Single(pipe);
    }
    
    [Fact]
    public void Test_Everything_Service()
    {
        var pipe = new Pipe
        {
            Cache.Everything<ServicePipeable>()
        };
        
        Assert.Single(pipe);
    }
}
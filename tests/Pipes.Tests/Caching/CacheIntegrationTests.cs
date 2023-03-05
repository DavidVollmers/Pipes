using Pipes.Caching;
using Pipes.Tests.Pipeables;

namespace Pipes.Tests.Caching;

public class CacheIntegrationTests
{
    [Fact]
    public void Test_Execute_CacheInput()
    {
        const int input = 4;
        var pipeable = new DelegatePipeable<int, int>(i => (int) i!, p => p.Pipe(p.Input * 2));
        var pipe = new Pipe<int, int>
        {
            Cache.Input(pipeable)
        };
        
        var result1 = pipe.Execute(input);
        Assert.Equal(8, result1);
        Assert.Equal(result1, pipe.Output);

        var result2 = pipe.Execute(result1);
        Assert.Equal(result1, result2);
        Assert.Equal(result1, pipe.Output);
        
        pipe.Reset();

        var result3 = pipe.Execute(result2);
        Assert.Equal(16, result3);
        Assert.Equal(result3, pipe.Output);
    }
    
    [Fact]
    public void Test_Execute_CacheOutput()
    {
        const int input = 4;
        var pipeable = new DelegatePipeable<int, int>(i => (int) i!, p => p.Pipe(p.Input * 2));
        var pipe = new Pipe<int, int>
        {
            Cache.Output(pipeable)
        };

        var result1 = pipe.Execute(input);
        Assert.Equal(8, result1);
        Assert.Equal(result1, pipe.Output);

        var result2 = pipe.Execute(result1);
        Assert.Equal(result1, result2);
        Assert.Equal(result1, pipe.Output);
        
        pipe.Reset();

        var result3 = pipe.Execute(result2);
        Assert.Equal(16, result3);
        Assert.Equal(result3, pipe.Output);
    }
    
    [Fact]
    public async Task Test_ExecuteAsync_CacheOutput()
    {
        const int input = 4;
        var pipeable = new DelegatePipeable<int, int>(i => (int) i!, async p => await p.PipeAsync(p.Input * 2));
        var pipe = new Pipe<int, int>
        {
            Cache.Output(pipeable)
        };

        var result1 = await pipe.ExecuteAsync(input);
        Assert.Equal(8, result1);
        Assert.Equal(result1, pipe.Output);

        var result2 = await pipe.ExecuteAsync(result1);
        Assert.Equal(result1, result2);
        Assert.Equal(result1, pipe.Output);
        
        pipe.Reset();

        var result3 = await pipe.ExecuteAsync(result2);
        Assert.Equal(16, result3);
        Assert.Equal(result3, pipe.Output);
    }
}
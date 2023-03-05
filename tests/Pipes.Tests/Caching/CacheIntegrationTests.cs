using Pipes.Caching;
using Pipes.Tests.Pipeables;

namespace Pipes.Tests.Caching;

public class CacheIntegrationTests
{
    [Fact]
    public void Test_CacheOutput()
    {
        var random = new Random();
        var pipeable = new DelegatePipeable<object, int>(i => i, p => p.Pipe(random.Next()));
        var pipe = new Pipe<int>
        {
            Cache.Output(pipeable)
        };

        var result1 = pipe.Execute();
        Assert.True(result1 > 0);
        Assert.Equal(result1, pipe.Output);

        var result2 = pipe.Execute();
        Assert.Equal(result1, result2);
        Assert.Equal(result1, pipe.Output);
        
        pipe.Reset();

        var result3 = pipe.Execute();
        Assert.NotEqual(result1, result3);
        Assert.Equal(result3, pipe.Output);
    }
}
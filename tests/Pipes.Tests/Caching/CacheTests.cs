using Pipes.Caching;
using Pipes.Tests.Pipeables;

namespace Pipes.Tests.Caching;

public class CacheTests
{
    [Fact]
    public void Test_Output()
    {
        var pipe = new Pipe
        {
            Cache.Output(new EmptyPipeable()),
            Cache.Output(new DelegatePipeable<int, string>(null, null))
        };
    }
}
using Pipes.Tests.Pipeables;

namespace Pipes.Tests;

public class PipeOutputTests
{
    [Fact]
    public async Task Test()
    {
        var input = true;
        var pipe = new Pipe<bool, bool>
        {
            new DelegatePipeable<bool, bool>(i => (bool)i!, p => p.Pipe(!p.Input))
        };

        var result = await pipe.ExecuteAsync(input);
        Assert.Equal(!input, result);
        Assert.Equal(!input, pipe.Output);
        Assert.Equal(!input, (bool)((PipeOutput)pipe).Output!);
    }
}
using Pipes.Tests.Pipeables;

namespace Pipes.Tests;

public class PipeTests
{
    [Fact]
    public void Test_Execute_NoPipeables()
    {
        var pipe = new Pipe();

        pipe.Execute();

        Assert.Null(pipe.Output);
    }

    [Fact]
    public async Task Test_ExecuteAsync_NoPipeables()
    {
        var pipe = new Pipe();

        await pipe.ExecuteAsync();

        Assert.Null(pipe.Output);
    }

    [Fact]
    public async Task Test_Output_IsCached()
    {
        var i = new Random().Next();
        var pipe = new Pipe<int, int>
        {
            new PipeableDelegate<int, int>(i => (int)i!, p => p.Pipe(p.Input * 2))
        };

        var result = await pipe.ExecuteAsync(i);
        Assert.Equal(i * 2, result);
        Assert.Equal(i * 2, pipe.Output);
    }
}
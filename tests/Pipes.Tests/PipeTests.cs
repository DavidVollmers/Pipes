using Pipes.Tests.Pipeables;

namespace Pipes.Tests;

public class PipeTests
{
    [Fact]
    public void Test_Execute_NoPipeables()
    {
        var pipe = new Pipe();

        pipe.Execute();
    }

    [Fact]
    public void Test_Execute_EmptyPipeable()
    {
        var emptyPipeable1 = new EmptyPipeable();

        var pipe = new Pipe
        {
            emptyPipeable1
        };

        var exception = Assert.Throws<InvalidOperationException>(() => pipe.Execute());
        Assert.Equal(
            "Pipe was not executed properly. Make sure to either call .Pipe() or .PipeAsync() when implementing custom pipeables.",
            exception.Message);
    }
}
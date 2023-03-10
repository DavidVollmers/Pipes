using Pipes.Tests.Pipeables;

namespace Pipes.Tests;

public class PipeExceptionTests
{
    [Fact]
    public void Test_Execute_PipeNotExecutedProperly()
    {
        var pipe = new Pipe
        {
            new EmptyPipeable()
        };

        var exception = Assert.Throws<InvalidOperationException>(() => pipe.Execute());
        Assert.Equal(
            "Pipe was not executed properly. Make sure to either call .Pipe() or .PipeAsync() when implementing custom pipeables.",
            exception.Message);
    }

    [Fact]
    public async Task Test_ExecuteAsync_PipeNotExecutedProperly()
    {
        var pipe = new Pipe
        {
            new EmptyPipeable()
        };

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => pipe.ExecuteAsync());
        Assert.Equal(
            "Pipe was not executed properly. Make sure to either call .Pipe() or .PipeAsync() when implementing custom pipeables.",
            exception.Message);
    }

    [Fact]
    public void Test_Execute_PipeAlreadyUsed()
    {
        var pipe = new Pipe
        {
            new DelegatePipeable(i => i, p =>
            {
                p.Pipe(null);
                p.Pipe(null);
            })
        };

        var exception = Assert.Throws<InvalidOperationException>(() => pipe.Execute());
        Assert.Equal(
            "Pipe already used. Make sure to only make one call to either .Pipe() or .PipeAsync() when implementing custom pipeables.",
            exception.Message);
    }

    [Fact]
    public async Task Test_ExecuteAsync_PipeAlreadyUsed()
    {
        var pipe = new Pipe
        {
            new DelegatePipeable(i => i, async p =>
            {
                await p.PipeAsync(null);
                await p.PipeAsync(null);
            })
        };

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => pipe.ExecuteAsync());
        Assert.Equal(
            "Pipe already used. Make sure to only make one call to either .Pipe() or .PipeAsync() when implementing custom pipeables.",
            exception.Message);
    }

    [Fact]
    public async Task Test_VerifyOutput_OutputTypeNotSupported()
    {
        const string output = "Not an Integer";
        var pipe = new Pipe<object, int>
        {
            new DelegatePipeable(i => i, p => p.Pipe(output))
        };

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => pipe.ExecuteAsync(0));
        Assert.Equal(
            $"Output of type \"{output.GetType().FullName}\" is not supported. Expected output type \"{typeof(int).FullName}\".",
            exception.Message);
    }

    [Fact]
    public void Test_Add_PipeableIsNull()
    {
        var pipe = new Pipe();

        var exception = Assert.Throws<ArgumentNullException>(() => pipe.Add(null!));
        Assert.Equal("pipeable", exception.ParamName);
    }
}
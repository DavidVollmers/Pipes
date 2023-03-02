using System.Collections;
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
    public void Test_Output_Default()
    {
        var objectPipe = new Pipe();
        var structPipe = new Pipe<object, int>();

        Assert.Equal(default, objectPipe.Output);
        Assert.Equal(default, structPipe.Output);
    }

    [Fact]
    public async Task Test_Output_Async()
    {
        var i = new Random().Next();
        var pipe = new Pipe<int, int>
        {
            new DelegatePipeable<int, int>(i => (int)i!, p => p.Pipe(p.Input * 2))
        };

        var result = await pipe.ExecuteAsync(i);
        Assert.Equal(i * 2, result);
        Assert.Equal(i * 2, pipe.Output);
    }

    [Fact]
    public void Test_Output()
    {
        var integer = new Random().Next();
        var pipe = new Pipe<int, int>
        {
            new DelegatePipeable<int, int>(i => (int)i!, p => p.Pipe(p.Input * 2))
        };

        var result = pipe.Execute(integer);
        Assert.Equal(integer * 2, result);
        Assert.Equal(integer * 2, pipe.Output);
    }

    [Fact]
    public void Test_Output_Null()
    {
        var pipe = new Pipe<int, int>
        {
            new DelegatePipeable(i => i, p => p.Pipe(null))
        };

        var result = pipe.Execute(1337);
        Assert.Equal(default, result);
        Assert.Equal(default, pipe.Output);
    }

    [Fact]
    public async Task Test_Reset()
    {
        var integer = new Random().Next();
        var pipe = new Pipe<int, int>
        {
            new DelegatePipeable<int, int>(i => (int)i!, p => p.Pipe(p.Input * 2))
        };

        var result = await pipe.ExecuteAsync(integer);
        Assert.Equal(integer * 2, result);
        Assert.Equal(integer * 2, pipe.Output);

        pipe.Reset();
        Assert.Equal(default, pipe.Output);
    }

    [Fact]
    public void Test_GetEnumerator()
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var pipe = new Pipe
        {
            new EmptyPipeable()
        };
        pipe.Add(new EmptyPipeable());

        Assert.Collection(pipe,
            result => Assert.IsType<EmptyPipeable>(result),
            result => Assert.IsType<EmptyPipeable>(result));
    }

    [Fact]
    public void Test_GetEnumerator_IEnumerableCast()
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var pipe = new Pipe
        {
            new EmptyPipeable()
        };
        pipe.Add(new EmptyPipeable());

        foreach (var result in (IEnumerable) pipe)
        {
            Assert.IsType<EmptyPipeable>(result);
        }
    }
}
namespace Pipes.Tests;

public class PipeTests
{
    [Fact]
    public void Test_Execute_Empty()
    {
        var pipe = new Pipe();
        
        pipe.Execute();
    }
    
    [Fact]
    public async Task Test_ExecuteAsync_Empty()
    {
        var pipe = new Pipe();
        
        await pipe.ExecuteAsync();
    }
}
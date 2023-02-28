using Pipes.IO;

namespace Pipes.Tests;

public class PipeIntegrationTests
{
    [Fact]
    public async Task Test_EnumerateAndReadFileContentAsync()
    {
        var expectedContent = "Lorem Ipsum dolor sit amet.";
        var path = Path.GetTempPath() + Guid.NewGuid() + ".txt";
        await File.WriteAllTextAsync(path, expectedContent);
        
        var pipe = new Pipe<string, Stream>
        {
            new EnumerateFiles(),
            new ReadFileContentAsync()
        };

        var result = await pipe.ExecuteAsync(path);
        Assert.NotNull(result);

        using var reader = new StreamReader(result!);
        var content = await reader.ReadToEndAsync();
        Assert.Equal(expectedContent, content);
    }
}
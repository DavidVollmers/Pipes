using Pipes.IO;

namespace Pipes.Tests;

public class PipeIntegrationTests
{
    [Fact]
    public async Task Test_EnumerateAndReadFileContentAsync()
    {
        var expectedContent = "Lorem Ipsum dolor sit amet.";
        var guid = Guid.NewGuid();
        var path = Path.GetTempPath() + guid + ".txt";
        await File.WriteAllTextAsync(path, expectedContent);
        
        var pipe = new Pipe<EnumerateFilesOptions, Stream>
        {
            new EnumerateFiles(),
            new ReadFileContentAsync()
        };

        var result = await pipe.ExecuteAsync(new EnumerateFilesOptions
        {
            WorkingDirectory = new FileInfo(path).Directory!,
            SearchPattern = $"*{guid}.txt"
        });
        Assert.NotNull(result);

        using var reader = new StreamReader(result!);
        var content = await reader.ReadToEndAsync();
        Assert.Equal(expectedContent, content);
    }
}
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
            (IEnumerable<string> paths) => paths.Single(),
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

        Assert.Equal(pipe.Output, result);
    }

    [Fact]
    public void Test_ExecuteSyncToAsync()
    {
        var input = new Random().Next();
        var pipe = new Pipe<int, int>
        {
            (int i) => i * 2,
            async (int i) => i + 1,
        };

        var result = pipe.Execute(input);
        Assert.Equal(input * 2 + 1, result);
    }

    [Fact]
    public async Task Test_ExecuteAsyncToSync()
    {
        var input = new Random().Next();
        var pipe = new Pipe<int, int>
        {
            async (int i) => i + 1,
            (int i) => i * 2,
        };

        var result = await pipe.ExecuteAsync(input);
        Assert.Equal((input + 1) * 2, result);
    }
}
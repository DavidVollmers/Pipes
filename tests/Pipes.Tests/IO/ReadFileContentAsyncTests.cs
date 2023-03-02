using System.Linq.Expressions;
using System.Text;
using Moq;
using Pipes.IO;

namespace Pipes.Tests.IO;

public class ReadFileContentAsyncTests
{
    [Fact]
    public void Test_ConvertInput_Null()
    {
        var pipeable = new ReadFileContentAsync();

        var result = pipeable.ConvertInput(null);
        Assert.Null(result);
    }

    [Fact]
    public void Test_ConvertInput_String()
    {
        var input = Path.GetTempFileName();

        var pipeable = new ReadFileContentAsync();

        var result = pipeable.ConvertInput(input);
        Assert.NotNull(result);
        Assert.NotNull(result!.File);
        Assert.Equal(input, result.File!.FullName);
    }

    [Fact]
    public void Test_ConvertInput_FileInfo()
    {
        var input = new FileInfo(Path.GetTempFileName());

        var pipeable = new ReadFileContentAsync();

        var result = pipeable.ConvertInput(input);
        Assert.NotNull(result);
        Assert.NotNull(result!.File);
        Assert.Equal(input.FullName, result.File!.FullName);
    }

    [Fact]
    public void Test_ConvertInput_SingleFileOptions()
    {
        var input = new SingleFileOptions
        {
            File = new FileInfo(Path.GetTempFileName())
        };

        var pipeable = new ReadFileContentAsync();

        var result = pipeable.ConvertInput(input);
        Assert.NotNull(result);
        Assert.NotNull(result!.File);
        Assert.Equal(input.File.FullName, result.File!.FullName);
    }

    [Fact]
    public void Test_ConvertInput_PipeInputNotSupportedException()
    {
        var input = 21528;

        var pipeable = new ReadFileContentAsync();

        Assert.Throws<PipeInputNotSupportedException>(() => pipeable.ConvertInput(input));
    }

    [Fact]
    public async Task Test_ExecuteAsync_FileIsNull()
    {
        var input = new SingleFileOptions();

        var pipe = new Mock<IPipe<SingleFileOptions, Stream>>();
        pipe.Setup(p => p.Input).Returns(input);

        var pipeable = new ReadFileContentAsync();

        var exception = await Assert.ThrowsAsync<PipeInputNullException>(() => pipeable.ExecuteAsync(pipe.Object));
        Assert.Equal("File", exception.InputName);

        pipe.Verify(p => p.Input, Times.Once);
        pipe.Verify(p => p.Pipe(It.IsAny<Stream>()), Times.Never);
        pipe.Verify(p => p.PipeAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Test_ExecuteAsync()
    {
        var expectedContent = "Lorem Ipsum dolor sit amet.";
        var guid = Guid.NewGuid();
        var path = Path.GetTempPath() + guid + ".txt";
        await File.WriteAllTextAsync(path, expectedContent);
        var input = new SingleFileOptions
        {
            File = new FileInfo(path)
        };

        var pipe = new Mock<IPipe<SingleFileOptions, Stream>>();
        pipe.Setup(p => p.Input).Returns(input);

        var pipeable = new ReadFileContentAsync();

        await pipeable.ExecuteAsync(pipe.Object);

        pipe.Verify(p => p.Input, Times.Exactly(2));
        pipe.Verify(p => p.Pipe(It.IsAny<Stream>()), Times.Never);
        pipe.Verify(
            p => p.PipeAsync(It.Is(AssertStreamContent(expectedContent)), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static Expression<Func<MemoryStream, bool>> AssertStreamContent(string expectedContent)
    {
        return stream => stream.ToArray().SequenceEqual(Encoding.UTF8.GetBytes(expectedContent));
    }
}
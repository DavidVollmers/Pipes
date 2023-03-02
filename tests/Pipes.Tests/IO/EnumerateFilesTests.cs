using Pipes.IO;

namespace Pipes.Tests.IO;

public class EnumerateFilesTests
{
    [Fact]
    public void Test_ConvertInput_Null()
    {
        var pipeable = new EnumerateFiles();

        var exception = Assert.Throws<PipeInputNullException>(() => pipeable.ConvertInput(null));
        Assert.Equal("input", exception.InputName);
    }

    [Fact]
    public void Test_ConvertInput_String()
    {
        var input = Guid.NewGuid().ToString();
        
        var pipeable = new EnumerateFiles();

        var result = pipeable.ConvertInput(input);
        Assert.NotNull(result);
        Assert.Equal(input, result!.SearchPattern);
    }

    [Fact]
    public void Test_ConvertInput_EnumerateFilesOptions()
    {
        var input = new EnumerateFilesOptions
        {
            SearchPattern = Guid.NewGuid().ToString()
        };
        
        var pipeable = new EnumerateFiles();

        var result = pipeable.ConvertInput(input);
        Assert.NotNull(result);
        Assert.Equal(input.SearchPattern, result!.SearchPattern);
    }

    [Fact]
    public void Test_ConvertInput_PipeInputNotSupportedException()
    {
        var input = 21528;

        var pipeable = new EnumerateFiles();

        Assert.Throws<PipeInputNotSupportedException>(() => pipeable.ConvertInput(input));
    }

    [Fact]
    public void Test_Execute_WorkingDirectoryIsNull()
    {
        var input = new EnumerateFilesOptions
        {
            WorkingDirectory = null!
        };

        var pipe = new Mock<IPipe<EnumerateFilesOptions, IEnumerable<string>>>();
        pipe.Setup(p => p.Input).Returns(input);

        var pipeable = new EnumerateFiles();

        var exception = Assert.Throws<PipeInputNullException>(() => pipeable.Execute(pipe.Object));
        Assert.Equal("WorkingDirectory", exception.InputName);

        pipe.Verify(p => p.Input, Times.Once);
        pipe.Verify(p => p.Pipe(It.IsAny<IEnumerable<string>>()), Times.Never);
        pipe.Verify(p => p.PipeAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Test_Execute_SearchPatternIsNull()
    {
        var input = new EnumerateFilesOptions
        {
            WorkingDirectory = new DirectoryInfo(Directory.GetCurrentDirectory())
        };

        var pipe = new Mock<IPipe<EnumerateFilesOptions, IEnumerable<string>>>();
        pipe.Setup(p => p.Input).Returns(input);

        var pipeable = new EnumerateFiles();

        var exception = Assert.Throws<PipeInputNullException>(() => pipeable.Execute(pipe.Object));
        Assert.Equal("SearchPattern", exception.InputName);

        pipe.Verify(p => p.Input, Times.Exactly(2));
        pipe.Verify(p => p.Pipe(It.IsAny<IEnumerable<string>>()), Times.Never);
        pipe.Verify(p => p.PipeAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Test_Execute()
    {
        var input = new EnumerateFilesOptions
        {
            WorkingDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()),
            SearchPattern = Guid.NewGuid().ToString()
        };

        var pipe = new Mock<IPipe<EnumerateFilesOptions, IEnumerable<string>>>();
        pipe.Setup(p => p.Input).Returns(input);

        var pipeable = new EnumerateFiles();

        pipeable.Execute(pipe.Object);

        pipe.Verify(p => p.Input, Times.Exactly(5));
        pipe.Verify(p => p.Pipe(It.Is<IEnumerable<string>>(s => !s.Any())), Times.Once);
        pipe.Verify(p => p.PipeAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Test_Execute_WithSearchOption()
    {
        var input = new EnumerateFilesOptions
        {
            WorkingDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()),
            SearchPattern = Guid.NewGuid().ToString(),
            SearchOption = SearchOption.AllDirectories
        };

        var pipe = new Mock<IPipe<EnumerateFilesOptions, IEnumerable<string>>>();
        pipe.Setup(p => p.Input).Returns(input);

        var pipeable = new EnumerateFiles();

        pipeable.Execute(pipe.Object);

        pipe.Verify(p => p.Input, Times.Exactly(5));
        pipe.Verify(p => p.Pipe(It.Is<IEnumerable<string>>(s => !s.Any())), Times.Once);
        pipe.Verify(p => p.PipeAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
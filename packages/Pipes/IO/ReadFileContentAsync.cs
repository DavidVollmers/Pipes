using Pipes.Abstractions;

namespace Pipes.IO;

public class ReadFileContentAsync : Pipeable<SingleFileInput, Stream>
{
    public override async Task<Stream?> ExecuteAsync(IPipe<SingleFileInput, Stream> pipe)
    {
        if (pipe.Input.File == null) throw new PipeInputNullException(nameof(SingleFileInput.File));

        var fileStream = File.OpenRead(pipe.Input.File.FullName);

        var stream = new MemoryStream();
        await fileStream.CopyToAsync(stream).ConfigureAwait(false);

        stream.Position = 0;
        return stream;
    }
}
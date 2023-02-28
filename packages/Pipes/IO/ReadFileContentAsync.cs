using Pipes.Abstractions;

namespace Pipes.IO;

public class ReadFileContentAsync : Pipeable<SingleFileInput, Stream>
{
    public override SingleFileInput ConvertInput(object input)
    {
        if (input is string path)
            return new SingleFileInput
            {
                File = new FileInfo(path)
            };

        if (input is FileInfo file)
            return new SingleFileInput
            {
                File = file
            };

        if (input is FileBasedInput fileBasedInput)
            return new SingleFileInput
            {
                WorkingDirectory = fileBasedInput.WorkingDirectory
            };

        throw new PipeInputNotSupportedException(input.GetType(), typeof(SingleFileInput));
    }

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
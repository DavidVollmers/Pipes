using Pipes.Abstractions;

namespace Pipes.IO;

public class ReadFileContentAsync : Pipeable<SingleFileOptions, Stream>
{
    public override SingleFileOptions? ConvertInput(object? input)
    {
        if (input == null) return null;

        if (input is string path)
            return new SingleFileOptions
            {
                File = new FileInfo(path)
            };

        if (input is IEnumerable<string> paths)
            return new SingleFileOptions
            {
                File = new FileInfo(paths.Single())
            };

        if (input is FileInfo file)
            return new SingleFileOptions
            {
                File = file
            };

        if (input is FileBasedOptions fileBasedInput)
            return new SingleFileOptions
            {
                WorkingDirectory = fileBasedInput.WorkingDirectory
            };

        throw new PipeInputNotSupportedException(input.GetType(), typeof(SingleFileOptions));
    }

    public override async Task ExecuteAsync(IPipe<SingleFileOptions, Stream> pipe,
        CancellationToken cancellationToken = default)
    {
        if (pipe.Input?.File == null) throw new PipeInputNullException(nameof(SingleFileOptions.File));

        var fileStream = File.OpenRead(pipe.Input.File.FullName);

        var stream = new MemoryStream();
        await fileStream.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);

        stream.Position = 0;

        await pipe.PipeAsync(stream, cancellationToken).ConfigureAwait(false);
    }
}
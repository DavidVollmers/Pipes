using Pipes.Abstractions;

namespace Pipes.IO;

public class ReadFileContentAsync : Pipeable<SingleFileOptions, Stream>
{
    public override SingleFileOptions? ConvertInput(object? input)
    {
        if (input == null) return null;
        
        if (InputConverter.TryConvertInput(input, out SingleFileOptions? convertedInput)) return convertedInput;

        if (InputConverter.TryConvertInput(input, out string? path))
            return new SingleFileOptions
            {
                File = new FileInfo(path!)
            };

        if (InputConverter.TryConvertInput(input, out FileInfo? file))
            return new SingleFileOptions
            {
                File = file
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
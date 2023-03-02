using Pipes.Input;

namespace Pipes.IO;

public class ReadFileContentAsync : Pipeable<SingleFileOptions, Stream>
{
    public override SingleFileOptions ConvertInput(object? input)
    {
        return InputConverter.ConvertInputByTypeMap(input, new TypeMap<SingleFileOptions>
        {
            (SingleFileOptions o) => o,
            (string path) => new SingleFileOptions
            {
                File = new FileInfo(path)
            },
            (FileInfo file) => new SingleFileOptions
            {
                File = file
            }
        });
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
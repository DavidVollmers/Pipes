using Pipes.Abstractions;

namespace Pipes.IO;

public class ReadFileContentAsync : IAsyncPipeable<SingleFileInput, Stream>
{
    public async Task ExecuteAsync(Pipeline<SingleFileInput, Stream> pipeline)
    {
        if (pipeline.Input.File == null) throw new PipelineInputNullException(nameof(SingleFileInput.File));

        var fileStream = File.OpenRead(pipeline.Input.File.FullName);
        
        var stream = new MemoryStream();
        await fileStream.CopyToAsync(stream).ConfigureAwait(false);

        stream.Position = 0;
        pipeline.Next(stream);
    }
}
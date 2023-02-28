using Pipes.Abstractions;

namespace Pipes.IO;

public sealed class EnumerateFiles : IPipeable<EnumerateFilesInput, IEnumerable<string>>
{
    public void Execute(Pipeline<EnumerateFilesInput, IEnumerable<string>> pipeline)
    {
        if (pipeline.Input.SearchPattern == null)
            throw new PipelineInputNullException(nameof(EnumerateFilesInput.SearchPattern));

        var paths =
            Directory.EnumerateFiles(pipeline.Input.WorkingDirectory.FullName, pipeline.Input.SearchPattern,
                pipeline.Input.SearchOption);

        pipeline.Next(paths);
    }
}
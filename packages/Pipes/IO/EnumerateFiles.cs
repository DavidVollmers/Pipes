using Pipes.Abstractions;

namespace Pipes.IO;

public sealed class EnumerateFiles : Pipeable<EnumerateFilesInput, IEnumerable<string>>
{
    public override IEnumerable<string> Execute(IPipe<EnumerateFilesInput, IEnumerable<string>> pipe)
    {
        if (pipe.Input.SearchPattern == null)
            throw new PipeInputNullException(nameof(EnumerateFilesInput.SearchPattern));

        var paths =
            Directory.EnumerateFiles(pipe.Input.WorkingDirectory.FullName, pipe.Input.SearchPattern,
                pipe.Input.SearchOption);

        return paths;
    }
}
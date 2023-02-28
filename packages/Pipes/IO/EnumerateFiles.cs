using Pipes.Abstractions;

namespace Pipes.IO;

public sealed class EnumerateFiles : Pipeable<EnumerateFilesInput, IEnumerable<string>>
{
    public override EnumerateFilesInput ConvertInput(object input)
    {
        if (input is EnumerateFilesInput enumerateFilesInput) return enumerateFilesInput;

        if (input is FileBasedInput fileBasedInput)
            return new EnumerateFilesInput
            {
                WorkingDirectory = fileBasedInput.WorkingDirectory
            };

        if (input is string searchPattern)
            return new EnumerateFilesInput
            {
                SearchPattern = searchPattern
            };

        throw new PipeInputNotSupportedException(input.GetType(), typeof(EnumerateFilesInput));
    }

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
namespace Pipes.IO;

public sealed class EnumerateFiles : Pipeable<EnumerateFilesOptions, IEnumerable<string>>
{
    public override EnumerateFilesOptions ConvertInput(object? input)
    {
        return InputConverter.ConvertInputByTypeMap(input, new TypeMap<EnumerateFilesOptions>
        {
            (EnumerateFilesOptions o) => o,
            (string s) => new EnumerateFilesOptions
            {
                SearchPattern = s
            }
        });
    }

    public override void Execute(IPipe<EnumerateFilesOptions, IEnumerable<string>> pipe)
    {
        if (pipe.Input?.WorkingDirectory == null)
            throw new PipeInputNullException(nameof(EnumerateFilesOptions.WorkingDirectory));
        if (pipe.Input?.SearchPattern == null)
            throw new PipeInputNullException(nameof(EnumerateFilesOptions.SearchPattern));

        var paths =
            Directory.EnumerateFiles(pipe.Input.WorkingDirectory.FullName, pipe.Input.SearchPattern,
                pipe.Input.SearchOption);

        pipe.Pipe(paths);
    }
}
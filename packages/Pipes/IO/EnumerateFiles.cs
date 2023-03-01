using Pipes.Abstractions;

namespace Pipes.IO;

public sealed class EnumerateFiles : Pipeable<EnumerateFilesOptions, IEnumerable<string>>
{
    public override EnumerateFilesOptions? ConvertInput(object? input)
    {
        if (input == null) return null;

        if (InputConverter.TryConvertInput(input, out EnumerateFilesOptions? convertedInput)) return convertedInput;

        if (InputConverter.TryConvertInput(input, out string? searchPattern))
            return new EnumerateFilesOptions
            {
                SearchPattern = searchPattern
            };

        throw new PipeInputNotSupportedException(input.GetType(), typeof(EnumerateFilesOptions));
    }

    public override void Execute(IPipe<EnumerateFilesOptions, IEnumerable<string>> pipe)
    {
        if (pipe.Input == null) throw new PipeInputNullException(nameof(EnumerateFilesOptions));
        if (pipe.Input.SearchPattern == null)
            throw new PipeInputNullException(nameof(EnumerateFilesOptions.SearchPattern));

        var paths =
            Directory.EnumerateFiles(pipe.Input.WorkingDirectory.FullName, pipe.Input.SearchPattern,
                pipe.Input.SearchOption);

        pipe.Pipe(paths);
    }
}
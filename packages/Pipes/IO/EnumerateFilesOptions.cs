namespace Pipes.IO;

public sealed class EnumerateFilesOptions : FileSystemBasedOptions
{
    public SearchOption SearchOption { get; set; }

    public string? SearchPattern { get; set; }
}
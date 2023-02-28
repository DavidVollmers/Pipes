namespace Pipes.IO;

public sealed class EnumerateFilesOptions : FileBasedOptions
{
    public SearchOption SearchOption { get; set; }

    public string? SearchPattern { get; set; }
}
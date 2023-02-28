namespace Pipes.IO;

public sealed class EnumerateFilesInput : FileBasedInput
{
    public SearchOption SearchOption { get; set; }

    public string? SearchPattern { get; set; }
}
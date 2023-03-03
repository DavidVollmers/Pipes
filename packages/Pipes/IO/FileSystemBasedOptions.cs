namespace Pipes.IO;

public class FileSystemBasedOptions
{
    public FileSystemBasedOptions()
    {
        WorkingDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
    }

    public DirectoryInfo WorkingDirectory { get; set; }
}
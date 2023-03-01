namespace Pipes.IO;

public class FileSystemBasedOptions
{
    public DirectoryInfo WorkingDirectory { get; set; }

    public FileSystemBasedOptions()
    {
        WorkingDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
    }
}
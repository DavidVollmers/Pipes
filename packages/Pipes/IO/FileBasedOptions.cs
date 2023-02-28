namespace Pipes.IO;

public class FileBasedOptions
{
    public DirectoryInfo WorkingDirectory { get; set; }

    public FileBasedOptions()
    {
        WorkingDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
    }
}
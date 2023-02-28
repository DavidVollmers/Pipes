namespace Pipes.IO;

public class FileBasedInput
{
    public DirectoryInfo WorkingDirectory { get; set; }

    public FileBasedInput()
    {
        WorkingDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
    }
}
using Pipes;
using Pipes.IO;

var expectedContent = "Lorem Ipsum dolor sit amet.";
var path = Path.GetTempPath() + Guid.NewGuid() + ".txt";
await File.WriteAllTextAsync(path, expectedContent);
        
var pipe = new Pipe<string, Stream>
{
    new EnumerateFiles(),
    new ReadFileContentAsync()
};

var result = await pipe.ExecuteAsync(path);

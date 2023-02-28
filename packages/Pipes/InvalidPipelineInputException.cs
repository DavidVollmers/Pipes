namespace Pipes;

public class InvalidPipelineInputException : Exception
{
    public InvalidPipelineInputException(string inputName, object inputValue) : base(
        $"Invalid pipeline input for \"{inputName}\": {inputValue}")
    {
    }
}
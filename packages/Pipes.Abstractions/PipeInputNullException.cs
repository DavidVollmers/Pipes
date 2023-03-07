namespace Pipes.Abstractions;

public class PipeInputNullException : InvalidPipeInputException
{
    public PipeInputNullException(string inputName) : base(inputName, "null")
    {
    }
}
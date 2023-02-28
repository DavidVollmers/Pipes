namespace Pipes;

public class PipeInputNullException : InvalidPipeInputException
{
    public PipeInputNullException(string inputName) : base(inputName, "null")
    {
    }
}
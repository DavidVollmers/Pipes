namespace Pipes;

public class PipelineInputNullException : InvalidPipelineInputException
{
    public PipelineInputNullException(string inputName) : base(inputName, "null")
    {
    }
}
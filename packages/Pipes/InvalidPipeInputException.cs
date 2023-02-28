namespace Pipes;

public class InvalidPipeInputException : Exception
{
    public InvalidPipeInputException(string inputName, object inputValue) : base(
        $"Invalid pipe input for \"{inputName}\": {inputValue}")
    {
    }
}
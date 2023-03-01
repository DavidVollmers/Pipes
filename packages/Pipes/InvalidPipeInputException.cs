namespace Pipes;

public class InvalidPipeInputException : Exception
{
    public string InputName { get; }

    public object InputValue { get; }
    
    public InvalidPipeInputException(string inputName, object inputValue) : base(
        $"Invalid pipe input for \"{inputName}\": {inputValue}")
    {
        InputName = inputName;
        InputValue = inputValue;
    }
}
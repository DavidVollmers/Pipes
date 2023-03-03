namespace Pipes.Abstractions;

public class InvalidPipeInputException : Exception
{
    public InvalidPipeInputException(string inputName, object inputValue) : base(
        $"Invalid pipe input for \"{inputName}\": {inputValue}")
    {
        InputName = inputName;
        InputValue = inputValue;
    }

    public string InputName { get; }

    public object InputValue { get; }
}
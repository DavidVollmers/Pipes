namespace Pipes;

public class MissingPipeException : Exception
{
    public MissingPipeException() : base("Nothing to pipe through.")
    {
    }
}
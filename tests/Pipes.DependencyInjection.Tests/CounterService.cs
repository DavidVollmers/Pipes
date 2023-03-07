namespace Pipes.DependencyInjection.Tests;

public class CounterService
{
    public int Value { get; private set; }

    public void Increment()
    {
        Value++;
    }
}
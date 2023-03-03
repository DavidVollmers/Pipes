using Microsoft.Extensions.DependencyInjection;
using Pipes.Abstractions;

namespace Pipes.DependencyInjection;

internal class PipeableType : IPipeable<object, object>
{
    private readonly ServiceInjection _serviceInjection;
    private IPipeable<object, object>? _pipeable;
    
    private IServiceProvider? _serviceProvider;

    public Type Type { get; }

    public PipeableType(Type type, ServiceInjection serviceInjection)
    {
        Type = type;
        _serviceInjection = serviceInjection;
    }

    public object? ConvertInput(object? input)
    {
        if (_serviceInjection == ServiceInjection.OnInput) ActivateType();

        return _pipeable!.ConvertInput(input);
    }

    public void Execute(IPipe<object, object?> pipe)
    {
        _pipeable!.Execute(pipe);
    }

    public Task ExecuteAsync(IPipe<object, object?> pipe, CancellationToken cancellationToken = default)
    {
        return _pipeable!.ExecuteAsync(pipe, cancellationToken);
    }

    public void Activate(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        if (_serviceInjection == ServiceInjection.OnActivation) ActivateType();
    }

    public void Reset()
    {
        _serviceProvider = null;
        _pipeable = null;
    }

    private void ActivateType()
    {
        _pipeable = (dynamic)_serviceProvider!.GetRequiredService(Type);
    }
}
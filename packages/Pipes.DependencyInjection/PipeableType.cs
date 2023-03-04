using Microsoft.Extensions.DependencyInjection;
using Pipes.Abstractions;

namespace Pipes.DependencyInjection;

internal class PipeableType : IPipeable<object, object>
{
    private ServiceInjection _serviceInjection;

    protected IServiceProvider? ServiceProvider { get; private set; }

    public object? Pipeable { get; private set; }
    public Type InputType { get; }
    public Type OutputType { get; }
    public Type Type { get; }

    public PipeableType(Type type)
    {
        Type = type;

        var pipeableInterface = type.GetInterface(typeof(IPipeable<,>).Name);
        if (pipeableInterface == null) throw new Exception("Type must be assignable to IPipeable.");

        InputType = pipeableInterface.GenericTypeArguments[0];
        OutputType = pipeableInterface.GenericTypeArguments[1];
    }

    public virtual object? ConvertInput(object? input)
    {
        if (_serviceInjection == ServiceInjection.OnInput) ActivateType();

        if (Pipeable == null)
            throw new InvalidOperationException(
                "Pipe was not activated before execution. Please use .Activate() before executing a pipe.");

        var convertInputMethod = Type.GetMethod(nameof(ConvertInput))!;
        return convertInputMethod.Invoke(Pipeable, new[] { input });
    }

    public virtual void Execute(IPipe<object, object?> pipe)
    {
        var genericPipe =
            TypeUtils.CreateGenericInstance(typeof(GenericPipeImplementation<,>), InputType, OutputType, pipe);

        var executeMethod = Type.GetMethod(nameof(Execute));
        executeMethod!.Invoke(Pipeable, new[] { genericPipe });
    }

    public virtual Task ExecuteAsync(IPipe<object, object?> pipe, CancellationToken cancellationToken = default)
    {
        var genericPipe =
            TypeUtils.CreateGenericInstance(typeof(GenericPipeImplementation<,>), InputType, OutputType, pipe);

        var executeAsyncMethod = Type.GetMethod(nameof(ExecuteAsync));
        return (Task)executeAsyncMethod!.Invoke(Pipeable, new[] { genericPipe, cancellationToken })!;
    }

    public void Activate(IServiceProvider serviceProvider, ServiceInjection serviceInjection)
    {
        _serviceInjection = serviceInjection;
        ServiceProvider = serviceProvider;

        if (_serviceInjection == ServiceInjection.OnActivation) ActivateType();
    }

    public void Reset()
    {
        ServiceProvider = null;
        Pipeable = null;
    }

    protected virtual void ActivateType()
    {
        Pipeable = ServiceProvider!.GetRequiredService(Type);
    }
}
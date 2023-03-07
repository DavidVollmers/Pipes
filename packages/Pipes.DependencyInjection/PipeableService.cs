using Microsoft.Extensions.DependencyInjection;
using Pipes.Abstractions;

namespace Pipes.DependencyInjection;

internal class PipeableService : IPipeable<object, object>, IPipeableService
{
    public PipeableService(Type type)
    {
        Type = type;

        var pipeableInterface = type.GetInterface(typeof(IPipeable<,>).Name);
        if (pipeableInterface == null) throw new Exception("Type must be assignable to IPipeable.");

        InputType = pipeableInterface.GenericTypeArguments[0];
        OutputType = pipeableInterface.GenericTypeArguments[1];
    }

    protected IServiceProvider? ServiceProvider { get; private set; }
    public object? Pipeable { get; private set; }
    public Type InputType { get; }
    public Type OutputType { get; }
    public Type Type { get; }

    public virtual object? ConvertInput(object? input)
    {
        if (ServiceInjection == ServiceInjection.OnInput) ActivateType();

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

    public virtual async Task ExecuteAsync(IPipe<object, object?> pipe, CancellationToken cancellationToken = default)
    {
        var genericPipe =
            TypeUtils.CreateGenericInstance(typeof(GenericPipeImplementation<,>), InputType, OutputType, pipe);

        var executeAsyncMethod = Type.GetMethod(nameof(ExecuteAsync));
        var task = (Task)executeAsyncMethod!.Invoke(Pipeable, new[] { genericPipe, cancellationToken })!;

        await task.ConfigureAwait(false);
    }

    public ServiceInjection ServiceInjection { get; private set; }

    public bool Activated { get; private set; }

    public ServiceLifetime ServiceLifetime { get; set; }

    public void Activate(IServiceProvider serviceProvider, ServiceInjection serviceInjection)
    {
        if (Activated)
            throw new InvalidOperationException(
                "Pipeable service already activated. Use .Reset() before activating it again.");

        ServiceInjection = serviceInjection;
        ServiceProvider = serviceProvider;

        if (ServiceInjection == ServiceInjection.OnActivation) ActivateType();
    }

    public void Reset()
    {
        ServiceProvider = null;
        Pipeable = null;

        Activated = false;
    }

    private void ActivateType()
    {
        Pipeable = ServiceProvider!.GetRequiredService(Type);

        Activated = true;
    }
}
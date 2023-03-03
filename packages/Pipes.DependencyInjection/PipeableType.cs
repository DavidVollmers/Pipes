using Microsoft.Extensions.DependencyInjection;
using Pipes.Abstractions;

namespace Pipes.DependencyInjection;

internal class PipeableType : IPipeable<object, object>
{
    private readonly ServiceInjection _serviceInjection;
    private readonly Type _inputType;
    private readonly Type _outputType;

    private object? _pipeable;
    private IServiceProvider? _serviceProvider;

    public Type Type { get; }

    public PipeableType(Type type, ServiceInjection serviceInjection)
    {
        Type = type;
        _serviceInjection = serviceInjection;

        var pipeableInterface = Type.GetInterface(typeof(IPipeable<,>).Name)!;
        _inputType = pipeableInterface.GenericTypeArguments[0];
        _outputType = pipeableInterface.GenericTypeArguments[1];
    }

    public object? ConvertInput(object? input)
    {
        if (_serviceInjection == ServiceInjection.OnInput) ActivateType();

        var convertInputMethod = Type.GetMethod(nameof(ConvertInput))!;
        return convertInputMethod.Invoke(_pipeable, new[] { input });
    }

    public void Execute(IPipe<object, object?> pipe)
    {
        var genericPipe = CreateGenericPipeImplementation(pipe);

        var executeMethod = Type.GetMethod(nameof(Execute));
        executeMethod!.Invoke(_pipeable, new[] { genericPipe });
    }

    public Task ExecuteAsync(IPipe<object, object?> pipe, CancellationToken cancellationToken = default)
    {
        var genericPipe = CreateGenericPipeImplementation(pipe);

        var executeAsyncMethod = Type.GetMethod(nameof(ExecuteAsync));
        return (Task)executeAsyncMethod!.Invoke(_pipeable, new[] { genericPipe, cancellationToken })!;
    }

    private object CreateGenericPipeImplementation(IPipe<object, object?> pipe)
    {
        var genericPipeType = typeof(GenericPipeImplementation<,>).MakeGenericType(_inputType, _outputType);
        return Activator.CreateInstance(genericPipeType, pipe)!;
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
        _pipeable = _serviceProvider!.GetRequiredService(Type);
    }
}
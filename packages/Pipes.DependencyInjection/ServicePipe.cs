using Microsoft.Extensions.DependencyInjection;
using Pipes.Abstractions;
using Pipes.Caching;
using Pipes.Input;

namespace Pipes.DependencyInjection;

public class ServicePipe : ServicePipe<object, object>
{
    public ServicePipe(ServiceInjection serviceInjection = ServiceInjection.OnActivation) : base(serviceInjection)
    {
    }

    public void Execute()
    {
        Execute(PipeInput.Empty);
    }

    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(PipeInput.Empty, cancellationToken);
    }
}

public class ServicePipe<TOutput> : ServicePipe<object, TOutput>
{
    public ServicePipe(ServiceInjection serviceInjection = ServiceInjection.OnActivation) : base(serviceInjection)
    {
    }

    public TOutput? Execute()
    {
        return Execute(PipeInput.Empty);
    }

    public Task<TOutput?> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(PipeInput.Empty, cancellationToken);
    }
}

public class ServicePipe<TInput, TOutput> : Pipe<TInput, TOutput>
{
    private readonly IList<PipeableType> _pipeableTypes = new List<PipeableType>();
    private readonly IList<PipeableCache<object, object>> _pipeableCaches = new List<PipeableCache<object, object>>();
    private readonly ServiceInjection _serviceInjection;

    private bool _activated;

    public ServicePipe(ServiceInjection serviceInjection = ServiceInjection.OnActivation)
    {
        _serviceInjection = serviceInjection;
    }

    public ServicePipe<TInput, TOutput> Add(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        Add(new PipeableType(type));
        return this;
    }

    public ServicePipe<TInput, TOutput> Activate(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        if (_activated)
            throw new InvalidOperationException(
                "Service pipe already activated. Use .Reset() before activating it again.");

        foreach (var pipeable in this)
        {
            switch (pipeable)
            {
                case PipeableType pipeableType:
                    pipeableType.Activate(serviceProvider, _serviceInjection);
                    _pipeableTypes.Add(pipeableType);
                    break;
                // case PipeableCache<object, object> { Pipeable: PipeableType pipeableServiceType } pipeableCache:
                //     pipeableServiceType.Activate(_scope.ServiceProvider, _serviceInjection);
                //     _pipeableTypes.Add(pipeableServiceType);
                //     _pipeableCaches.Add(pipeableCache);
                //     break;
            }
        }

        _activated = true;

        return this;
    }

    public override void Reset()
    {
        foreach (var pipeableType in _pipeableTypes) pipeableType.Reset();

        _pipeableTypes.Clear();

        foreach (var pipeableCache in _pipeableCaches) pipeableCache.Clear();

        _pipeableCaches.Clear();

        base.Reset();

        _activated = false;
    }
}
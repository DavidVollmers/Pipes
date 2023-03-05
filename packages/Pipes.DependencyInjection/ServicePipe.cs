using Microsoft.Extensions.DependencyInjection;
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

public class ServicePipe<TInput, TOutput> : Pipe<TInput, TOutput>, IServiceActivation
{
    private readonly IList<PipeableType> _pipeableTypes = new List<PipeableType>();
    private readonly ServiceInjection _serviceInjection;

    public bool Activated { get; private set; }

    public ServiceLifetime ServiceLifetime { get; internal set; }

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

    public void Activate(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        if (Activated)
            throw new InvalidOperationException(
                "Service pipe already activated. Use .Reset() before activating it again.");

        foreach (var pipeable in this)
        {
            if (pipeable is not PipeableType pipeableType) continue;

            pipeableType.Activate(serviceProvider, _serviceInjection);
            _pipeableTypes.Add(pipeableType);
        }

        Activated = true;
    }

    public override void Reset()
    {
        foreach (var pipeableType in _pipeableTypes) pipeableType.Reset();

        _pipeableTypes.Clear();

        base.Reset();

        Activated = false;
    }
}
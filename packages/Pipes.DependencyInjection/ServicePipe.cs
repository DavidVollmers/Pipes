using Microsoft.Extensions.DependencyInjection;
using Pipes.Abstractions;
using Pipes.DependencyInjection.Caching;
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

public class ServicePipe<TInput, TOutput> : Pipe<TInput, TOutput>, IServicePipe
{
    private readonly IList<IPipeableService> _pipeableServices = new List<IPipeableService>();

    public ServicePipe(ServiceInjection serviceInjection = ServiceInjection.OnActivation)
    {
        ServiceInjection = serviceInjection;
    }

    public ServiceInjection ServiceInjection { get; }

    public bool Activated { get; private set; }

    public ServiceLifetime ServiceLifetime { get; internal set; }

    public void Activate(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        if (Activated)
            throw new InvalidOperationException(
                "Service pipe already activated. Use .Reset() before activating it again.");

        foreach (var pipeable in this)
        {
            if (pipeable is not PipeableService pipeableService) continue;

            pipeableService.Activate(serviceProvider, ServiceInjection);
        }

        Activated = true;
    }

    public override void Reset()
    {
        base.Reset();

        foreach (var pipeableService in _pipeableServices) pipeableService.Reset();

        Activated = false;
    }

    IEnumerator<IPipeableService> IEnumerable<IPipeableService>.GetEnumerator()
    {
        return _pipeableServices.GetEnumerator();
    }

    public ServicePipe<TInput, TOutput> Add(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        Add(new PipeableService(type));
        return this;
    }

    public override Pipe<TInput, TOutput> Add(IPipeable<object, object> pipeable)
    {
        base.Add(pipeable);
        if (pipeable is PipeableService pipeableService) _pipeableServices.Add(pipeableService);
        return this;
    }
}
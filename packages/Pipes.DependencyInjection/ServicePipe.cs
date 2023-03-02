using Microsoft.Extensions.DependencyInjection;
using Pipes.Input;

namespace Pipes.DependencyInjection;

public class ServicePipe : ServicePipe<object, object>
{
    public void Execute()
    {
        Execute(PipeInput.Empty);
    }

    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(PipeInput.Empty, cancellationToken);
    }
}

public class ServicePipe<TInput, TOutput> : Pipe<TInput, TOutput>, IDisposable
{
    private readonly IList<PipeableType> _pipeableTypes = new List<PipeableType>();
    private readonly ServiceInjection _serviceInjection;

    private IServiceScope? _scope;

    public ServicePipe(ServiceInjection serviceInjection)
    {
        _serviceInjection = serviceInjection;
    }

    public ServicePipe() : this(ServiceInjection.OnActivation)
    {
    }

    public ServicePipe<TInput, TOutput> Add(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        var pipeableType = new PipeableType(type, _serviceInjection);
        _pipeableTypes.Add(pipeableType);
        Add(pipeableType);
        return this;
    }

    public ServicePipe<TInput, TOutput> Activate(IServiceProvider serviceProvider)
    {
        if (_scope != null)
            throw new InvalidOperationException(
                "Service pipe already activated. Use .Reset() before activating it again.");

        _scope = serviceProvider.CreateScope();

        foreach (var pipeableType in _pipeableTypes)
        {
            pipeableType.Activate(_scope.ServiceProvider);
        }

        return this;
    }

    public override void Reset()
    {
        foreach (var pipeableType in _pipeableTypes)
        {
            pipeableType.Reset();
        }

        if (_scope != null)
        {
            _scope.Dispose();
            _scope = null;
        }

        base.Reset();
    }

    public void Dispose()
    {
        _scope?.Dispose();
        
        GC.SuppressFinalize(this);
    }
}
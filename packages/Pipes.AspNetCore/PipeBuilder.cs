using Pipes.DependencyInjection;

namespace Pipes.AspNetCore;

public sealed class PipeBuilder
{
    private readonly IList<IServiceActivation> _serviceActivations = new List<IServiceActivation>();

    internal IEnumerable<IServiceActivation> ServiceActivations => _serviceActivations;

    internal PipeBuilder()
    {
    }

    public PipeBuilder Add<TInput, TOutput>(Pipe<TInput, TOutput> pipe)
    {
        if (pipe == null) throw new ArgumentNullException(nameof(pipe));
        if (pipe is ServicePipe<TInput, TOutput> servicePipe) _serviceActivations.Add(servicePipe);
        return this;
    }
}
using Microsoft.Extensions.DependencyInjection;
using Pipes.DependencyInjection;

namespace Pipes.AspNetCore;

public sealed class PipeBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private readonly IList<IServicePipe> _servicePipes = new List<IServicePipe>();

    internal PipeBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    internal IEnumerable<IServicePipe> ServicePipes => _servicePipes;

    public PipeBuilder Add<TInput, TOutput>(Pipe<TInput, TOutput> pipe)
    {
        if (pipe == null) throw new ArgumentNullException(nameof(pipe));
        _serviceCollection.AddTransient(pipe);
        if (pipe is ServicePipe<TInput, TOutput> servicePipe) _servicePipes.Add(servicePipe);
        return this;
    }
}
using Microsoft.Extensions.DependencyInjection;
using Pipes.DependencyInjection;

namespace Pipes.AspNetCore;

public sealed class PipeBuilder
{
    private readonly IServiceCollection _serviceCollection;

    internal PipeBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public PipeBuilder Add<TInput, TOutput>(Pipe<TInput, TOutput> pipe, ServiceLifetime serviceLifetime)
    {
        if (pipe == null) throw new ArgumentNullException(nameof(pipe));
        _serviceCollection.Add(pipe, serviceLifetime);
        return this;
    }

    public PipeBuilder AddTransient<TInput, TOutput>(Pipe<TInput, TOutput> pipe)
    {
        return Add(pipe, ServiceLifetime.Transient);
    }

    public PipeBuilder AddScoped<TInput, TOutput>(Pipe<TInput, TOutput> pipe)
    {
        return Add(pipe, ServiceLifetime.Scoped);
    }

    public PipeBuilder AddSingleton<TInput, TOutput>(Pipe<TInput, TOutput> pipe)
    {
        return Add(pipe, ServiceLifetime.Singleton);
    }
}
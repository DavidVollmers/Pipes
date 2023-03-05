using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pipes.DependencyInjection;

namespace Pipes.AspNetCore;

//TODO https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-7.0#middleware-order
internal class ServiceActivationMiddleware : IMiddleware
{
    private readonly IServiceActivation[] _serviceActivations;

    public ServiceActivationMiddleware(IEnumerable<IServiceActivation> serviceActivations)
    {
        _serviceActivations = serviceActivations.ToArray();
    }

    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        foreach (var service in _serviceActivations)
        {
            if (service.Activated) continue;
            
            service.Activate(context.RequestServices);
        }
        
        next(context);

        foreach (var service in _serviceActivations)
        {
            if (service.ServiceLifetime != ServiceLifetime.Scoped) continue;
            
            service.Reset();
        }
        
        return Task.CompletedTask;
    }
}
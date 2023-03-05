using Microsoft.AspNetCore.Http;
using Pipes.DependencyInjection;

namespace Pipes.AspNetCore;

//TODO https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-7.0#middleware-order
internal class ServiceActivationMiddleware : IMiddleware
{
    private readonly IServicePipe[] _servicePipes;

    public ServiceActivationMiddleware(IEnumerable<IServicePipe> serviceActivations)
    {
        _servicePipes = serviceActivations.ToArray();
    }

    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        foreach (var servicePipe in _servicePipes) servicePipe.EnsureScopeActivation(context.RequestServices);

        next(context);

        foreach (var servicePipe in _servicePipes) servicePipe.EnsureScopeReset(context.RequestServices);

        return Task.CompletedTask;
    }
}
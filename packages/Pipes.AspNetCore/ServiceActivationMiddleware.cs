using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pipes.DependencyInjection;

namespace Pipes.AspNetCore;

//TODO https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-7.0#middleware-order
internal class ServiceActivationMiddleware : IMiddleware
{
    private readonly IServicePipe[] _servicePipes;

    public ServiceActivationMiddleware(IServiceProvider serviceProvider)
    {
        var servicePipes = serviceProvider.GetService<IEnumerable<IServicePipe>>();
        _servicePipes = servicePipes == null ? Array.Empty<IServicePipe>() : servicePipes.ToArray();
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        foreach (var servicePipe in _servicePipes) servicePipe.EnsureScopeActivation(context.RequestServices);

        await next(context).ConfigureAwait(false);

        foreach (var servicePipe in _servicePipes) servicePipe.EnsureScopeReset();
    }
}
using Microsoft.AspNetCore.Http;

namespace Pipes.AspNetCore;

//TODO https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-7.0#middleware-order
internal class PipeActivationMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        next(context);
        
        return Task.CompletedTask;
    }
}
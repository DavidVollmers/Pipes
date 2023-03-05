using Microsoft.AspNetCore.Builder;

namespace Pipes.AspNetCore;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UsePipes(this IApplicationBuilder applicationBuilder)
    {
        if (applicationBuilder == null) throw new ArgumentNullException(nameof(applicationBuilder));

        applicationBuilder.UseMiddleware<ServiceActivationMiddleware>();

        return applicationBuilder;
    }
}
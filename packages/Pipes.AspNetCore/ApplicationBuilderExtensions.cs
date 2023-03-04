using Microsoft.AspNetCore.Builder;

namespace Pipes.AspNetCore;

public static class ApplicationBuilderExtensions
{
    public static PipeBuilder UsePipes(this IApplicationBuilder applicationBuilder)
    {
        if (applicationBuilder == null) throw new ArgumentNullException(nameof(applicationBuilder));

        var pipeBuilder = new PipeBuilder();

        applicationBuilder.UseMiddleware<PipeActivationMiddleware>(pipeBuilder);
        
        return pipeBuilder;
    }
}
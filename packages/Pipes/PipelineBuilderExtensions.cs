using Pipes.Abstractions;

namespace Pipes;

public static class PipelineBuilderExtensions
{
    public static PipelineBuilder Add<TInput, TOutput>(this PipelineBuilder builder, Pipeable<TInput, TOutput> pipeable)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        builder.Add(pipeable);
        return builder;
    }
}
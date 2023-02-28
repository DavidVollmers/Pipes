using Pipes.Abstractions;

namespace Pipes;

public static class PipeBuilderExtensions
{
    public static PipeBuilder Add<TInput, TOutput>(this PipeBuilder builder, Pipeable<TInput, TOutput> pipeable)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        builder.Add(pipeable);
        return builder;
    }
}
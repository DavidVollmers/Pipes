using Pipes.Abstractions;

namespace Pipes;

public static class PipeExtensions
{
    public static Pipe<TPipeInput, TPipeOutput> Add<TInput, TOutput, TPipeInput, TPipeOutput>(
        this Pipe<TPipeInput, TPipeOutput> builder, Pipeable<TInput, TOutput> pipeable)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        if (pipeable == null) throw new ArgumentNullException(nameof(pipeable));
        builder.Add(new GenericPipeable<TInput, TOutput>(pipeable));
        return builder;
    }
}
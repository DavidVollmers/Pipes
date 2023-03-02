﻿namespace Pipes;

public static class PipeExtensions
{
    public static Pipe<TPipeInput, TPipeOutput> Add<TInput, TOutput, TPipeInput, TPipeOutput>(
        this Pipe<TPipeInput, TPipeOutput> pipe, IPipeable<TInput, TOutput> pipeable)
    {
        if (pipe == null) throw new ArgumentNullException(nameof(pipe));
        if (pipeable == null) throw new ArgumentNullException(nameof(pipeable));
        pipe.Add(new GenericPipeable<TInput, TOutput>(pipeable));
        return pipe;
    }

    public static Pipe<TPipeInput, TPipeOutput> Add<TInput, TOutput, TPipeInput, TPipeOutput>(
        this Pipe<TPipeInput, TPipeOutput> pipe, Func<TInput, TOutput?> pipeable)
    {
        if (pipe == null) throw new ArgumentNullException(nameof(pipe));
        if (pipeable == null) throw new ArgumentNullException(nameof(pipeable));
        pipe.Add(new PipeableDelegate<TInput, TOutput>(pipeable));
        return pipe;
    }

    public static Pipe<TPipeInput, TPipeOutput> Add<TInput, TOutput, TPipeInput, TPipeOutput>(
        this Pipe<TPipeInput, TPipeOutput> pipe, Func<TInput, Task<TOutput?>> pipeable)
    {
        if (pipe == null) throw new ArgumentNullException(nameof(pipe));
        if (pipeable == null) throw new ArgumentNullException(nameof(pipeable));
        pipe.Add(new AsyncPipeableDelegate<TInput, TOutput>(pipeable));
        return pipe;
    }
}
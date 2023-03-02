using Pipes.Abstractions;

namespace Pipes.DependencyInjection;

public static class ServicePipeExtensions
{
    public static ServicePipe<TPipeInput, TPipeOutput> Add<TType, TInput, TOutput, TPipeInput, TPipeOutput>(
        this ServicePipe<TPipeInput, TPipeOutput> servicePipe) where TType : IPipeable<TInput, TOutput>
    {
        if (servicePipe == null) throw new ArgumentNullException(nameof(servicePipe));
        return servicePipe.Add(typeof(TType));
    }
}
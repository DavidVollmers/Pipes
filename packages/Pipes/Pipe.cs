namespace Pipes;

public class Pipe<TInput, TOutput> : PipeBuilder
{
    public TOutput? Execute(TInput input)
    {
        throw new NotImplementedException();
    }

    public async Task<TOutput?> ExecuteAsync(TInput input, CancellationToken cancellationToken = default)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        
        var pipe = Build(input);
        if (pipe == null) return default;
        
        await pipe.PipeAsync(input, cancellationToken).ConfigureAwait(false);
        
        //TODO
        return default;
    }
}
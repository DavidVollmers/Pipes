namespace Pipes.Caching;

[Flags]
internal enum CacheFlags
{
    None = 0,
    Input = 1,
    Output = 2
}
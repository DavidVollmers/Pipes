using Pipes.Abstractions;
using Pipes.Examples.AspNetCore.Models;
using Pipes.Examples.AspNetCore.Storage;

namespace Pipes.Examples.AspNetCore.Pipes.Pipeables;

public class GetAllTodoAsync : Pipeable<object, IEnumerable<TodoItem>>
{
    private readonly IStorageContext _storage;

    public GetAllTodoAsync(IStorageContext storage)
    {
        _storage = storage;
    }
    
    public override object? ConvertInput(object? input) => input;

    public override async Task ExecuteAsync(IPipe<object, IEnumerable<TodoItem>?> pipe,
        CancellationToken cancellationToken = default)
    {
        var items = await _storage.GetAllTodoItemsAsync();

        await pipe.PipeAsync(items, cancellationToken);
    }
}
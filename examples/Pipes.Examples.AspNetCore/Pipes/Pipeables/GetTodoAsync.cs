using Pipes.Abstractions;
using Pipes.Examples.AspNetCore.Models;
using Pipes.Examples.AspNetCore.Storage;
using Pipes.Input;

namespace Pipes.Examples.AspNetCore.Pipes.Pipeables;

public class GetTodoAsync : Pipeable<Guid, TodoItem?>
{
    private readonly IStorageContext _storage;

    public GetTodoAsync(IStorageContext storage)
    {
        _storage = storage;
    }

    public override Guid ConvertInput(object? input) => InputConverter.ConvertInput<Guid>(input);

    public override async Task ExecuteAsync(IPipe<Guid, TodoItem?> pipe, CancellationToken cancellationToken = default)
    {
        var item = await _storage.GetTodoItemAsync(pipe.Input);

        await pipe.PipeAsync(item, cancellationToken);
    }
}
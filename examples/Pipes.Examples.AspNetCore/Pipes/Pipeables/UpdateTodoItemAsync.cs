using Pipes.Abstractions;
using Pipes.Examples.AspNetCore.Models;
using Pipes.Examples.AspNetCore.Storage;
using Pipes.Input;

namespace Pipes.Examples.AspNetCore.Pipes.Pipeables;

public class UpdateTodoItemAsync : Pipeable<UpdateTodoItemRequest, TodoItem>
{
    private readonly IStorageContext _storageContext;

    public UpdateTodoItemAsync(IStorageContext storageContext)
    {
        _storageContext = storageContext;
    }

    public override UpdateTodoItemRequest ConvertInput(object? input) =>
        InputConverter.ConvertInput<UpdateTodoItemRequest>(input);

    public override async Task ExecuteAsync(IPipe<UpdateTodoItemRequest, TodoItem?> pipe,
        CancellationToken cancellationToken = default)
    {
        var item = await RequestPipes.Todo.Get.ExecuteAsync(pipe.Input!.Id, cancellationToken);

        item!.Todo = pipe.Input.Todo;
        item.IsPublic = pipe.Input.IsPublic;
        item.IsDone = pipe.Input.IsDone;
        
        await _storageContext.UpdateTodoItemAsync(item);

        await pipe.PipeAsync(item, cancellationToken);
    }
}
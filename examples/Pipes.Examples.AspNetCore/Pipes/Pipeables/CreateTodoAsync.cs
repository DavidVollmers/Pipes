using Pipes.Abstractions;
using Pipes.Examples.AspNetCore.Models;
using Pipes.Examples.AspNetCore.Storage;
using Pipes.Input;

namespace Pipes.Examples.AspNetCore.Pipes.Pipeables;

public class CreateTodoAsync : Pipeable<CreateTodoItemRequest, TodoItem>
{
    private readonly IStorageContext _storageContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateTodoAsync(IStorageContext storageContext, IHttpContextAccessor httpContextAccessor)
    {
        _storageContext = storageContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public override CreateTodoItemRequest ConvertInput(object? input) =>
        InputConverter.ConvertInput<CreateTodoItemRequest>(input);

    public override async Task ExecuteAsync(IPipe<CreateTodoItemRequest, TodoItem?> pipe,
        CancellationToken cancellationToken = default)
    {
        //TODO: Usually you also want to make sure the user is authenticated...
        var createdBy = _httpContextAccessor.HttpContext!.User.Identity!.Name!;

        var item = await _storageContext.CreateTodoItemAsync(pipe.Input!.Todo!, createdBy, pipe.Input!.IsPublic,
            pipe.Input!.IsDone);

        await pipe.PipeAsync(item, cancellationToken);
    }
}
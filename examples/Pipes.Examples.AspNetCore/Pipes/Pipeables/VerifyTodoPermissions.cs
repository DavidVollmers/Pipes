using Pipes.Abstractions;
using Pipes.Examples.AspNetCore.Models;
using Pipes.Input;

namespace Pipes.Examples.AspNetCore.Pipes.Pipeables;

public class VerifyTodoPermissions : Pipeable<IEnumerable<TodoItem>, IEnumerable<TodoItem>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VerifyTodoPermissions(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override IEnumerable<TodoItem> ConvertInput(object? input) =>
        InputConverter.ConvertInputToEnumerable<TodoItem>(input);

    public override void Execute(IPipe<IEnumerable<TodoItem>, IEnumerable<TodoItem>> pipe)
    {
        if (pipe.Input == null || pipe.Input.All(i => i.IsPublic))
        {
            pipe.Pipe(pipe.Input);
            return;
        }

        //TODO: Usually you also want to make sure the user is authenticated...
        // if (!(_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false))
        // {
        //     pipe.Pipe(Array.Empty<TodoItem>());
        //     return;
        // }

        var requestedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name;
        if (requestedBy == null && pipe.Input.All(i => i.CreatedBy == requestedBy))
        {
            pipe.Pipe(Array.Empty<TodoItem>());
            return;
        }

        pipe.Pipe(pipe.Input.Where(i => i.IsPublic || i.CreatedBy == requestedBy));
    }
}
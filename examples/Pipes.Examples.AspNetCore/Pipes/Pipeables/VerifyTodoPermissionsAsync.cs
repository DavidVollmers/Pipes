using Pipes.Abstractions;
using Pipes.Examples.AspNetCore.Models;
using Pipes.Input;

namespace Pipes.Examples.AspNetCore.Pipes.Pipeables;

public class VerifyTodoPermissionsAsync : Pipeable<TodoItem?, TodoItem?>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VerifyTodoPermissionsAsync(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override TodoItem? ConvertInput(object? input) => InputConverter.ConvertInput<TodoItem?>(input);

    public override async Task ExecuteAsync(IPipe<TodoItem?, TodoItem?> pipe,
        CancellationToken cancellationToken = default)
    {
        if (pipe.Input == null || pipe.Input.IsPublic)
        {
            await pipe.PipeAsync(pipe.Input, cancellationToken);
            return;
        }

        //TODO: Usually you also want to make sure the user is authenticated...
        // if (!(_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false))
        // {
        //     await pipe.PipeAsync(null, cancellationToken);
        //     return;
        // }

        var requestedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name;
        if (requestedBy == null || pipe.Input.CreatedBy != requestedBy)
        {
            await pipe.PipeAsync(null, cancellationToken);
            return;
        }

        await pipe.PipeAsync(pipe.Input, cancellationToken);
    }
}
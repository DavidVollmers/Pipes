using Microsoft.AspNetCore.Mvc;
using Pipes.Examples.AspNetCore.Models;
using Pipes.Examples.AspNetCore.Pipes;

namespace Pipes.Examples.AspNetCore.Controllers;

[ApiController]
[Route("api/todo")]
public class TodoController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;

    public TodoController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TodoItem>> GetTodoItemAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await RequestPipes.Todo.Get.Activate(_serviceProvider).ExecuteAsync(id, cancellationToken);

        if (item == null) return NotFound();

        return Ok(item);
    }
}
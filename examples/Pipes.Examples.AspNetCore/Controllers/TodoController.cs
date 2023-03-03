using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Pipes.Examples.AspNetCore.Models;
using Pipes.Examples.AspNetCore.Pipes;

namespace Pipes.Examples.AspNetCore.Controllers;

[ApiController]
[Route("api/todo")]
public class TodoController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<TodoItem>> GetAllTodoItemAsync([FromQuery] string? requestedBy = null,
        CancellationToken cancellationToken = default)
    {
        if (requestedBy != null) Login(requestedBy);

        var items = await RequestPipes.Todo.GetAll.ExecuteAsync(cancellationToken);

        return Ok(items);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TodoItem>> GetTodoItemAsync(Guid id, [FromQuery] string? requestedBy = null,
        CancellationToken cancellationToken = default)
    {
        if (requestedBy != null) Login(requestedBy);

        var item = await RequestPipes.Todo.Get.ExecuteAsync(id, cancellationToken);

        if (item == null) return NotFound();

        return Ok(item);
    }

    private void Login(string user)
    {
        //TODO: Usually this is done via authentication...
        var nameClaim = new Claim(ClaimTypes.Name, user);
        var identity = new ClaimsIdentity(new[] { nameClaim });
        HttpContext.User = new ClaimsPrincipal(new[] { identity });
    }
}
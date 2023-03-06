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
    public async Task<ActionResult<TodoItem[]>> GetAllTodoItemAsync([FromQuery] string? requestedBy = null,
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

    [HttpPost]
    public async Task<ActionResult<TodoItem>> CreateTodoItemAsync([FromBody] CreateTodoItemRequest request,
        [FromQuery] string? requestedBy = null, CancellationToken cancellationToken = default)
    {
        if (requestedBy == null) return Unauthorized();

        Login(requestedBy);

        var item = await RequestPipes.Todo.Create.ExecuteAsync(request, cancellationToken);

        return Ok(item);
    }

    [HttpPost("{id:guid}")]
    public async Task<ActionResult<TodoItem>> UpdateTodoItemAsync(Guid id, [FromBody] UpdateTodoItemRequest request,
        [FromQuery] string? requestedBy = null, CancellationToken cancellationToken = default)
    {
        if (requestedBy != null) Login(requestedBy);

        if (id != request.Id) return BadRequest();

        var existing = await RequestPipes.Todo.Get.ExecuteAsync(id, cancellationToken).ConfigureAwait(false);

        if (existing == null) return NotFound();

        var item = await RequestPipes.Todo.Update.ExecuteAsync(request, cancellationToken);

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
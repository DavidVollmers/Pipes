using System.ComponentModel.DataAnnotations;

namespace Pipes.Examples.AspNetCore.Models;

public class UpdateTodoItemRequest
{
    [Required] public Guid Id { get; set; }

    [Required] public string? Todo { get; set; }

    public bool IsPublic { get; set; }

    public bool IsDone { get; set; }
}
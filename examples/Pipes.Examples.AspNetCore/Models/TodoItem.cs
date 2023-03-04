namespace Pipes.Examples.AspNetCore.Models;

public class TodoItem
{
    public Guid Id { get; set; }

    public string? Todo { get; set; }

    public bool IsPublic { get; set; }

    public string? CreatedBy { get; set; }

    public bool IsDone { get; set; }
}
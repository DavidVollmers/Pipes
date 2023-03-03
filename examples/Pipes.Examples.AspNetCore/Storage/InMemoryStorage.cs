using Pipes.Examples.AspNetCore.Models;

namespace Pipes.Examples.AspNetCore.Storage;

public class InMemoryStorage : IStorageContext
{
    private static readonly IList<TodoItem> Items = new List<TodoItem>
    {
        new()
        {
            Id = Guid.Parse("ea8e25e1-36da-4dcb-93d2-d5d8b2c680b5"),
            Todo = "Build some pipes",
            IsPublic = true,
            IsDone = true,
            CreatedBy = "github@david.vollmers.org"
        },
        new()
        {
            Id = Guid.Parse("7ae5817a-9c95-4610-b65e-a41049ed5f3b"),
            Todo = "Oh noes! You breached my security layer...",
            IsPublic = false,
            IsDone = true,
            CreatedBy = "github@david.vollmers.org"
        }
    };

    public Task<TodoItem?> GetTodoItemAsync(Guid id)
    {
        var item = Items.FirstOrDefault(i => i.Id == id);
        return Task.FromResult(item);
    }
}
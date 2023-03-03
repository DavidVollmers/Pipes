using Pipes.Examples.AspNetCore.Models;

namespace Pipes.Examples.AspNetCore.Storage;

public class InMemoryStorage : IStorageContext
{
    private static readonly IList<TodoItem> Items = new List<TodoItem>
    {
        new TodoItem
        {
            Id = Guid.Parse("ea8e25e1-36da-4dcb-93d2-d5d8b2c680b5"),
            Todo = "Build some pipes",
            IsPublic = true,
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
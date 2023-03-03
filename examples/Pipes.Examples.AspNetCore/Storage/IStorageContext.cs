using Pipes.Examples.AspNetCore.Models;

namespace Pipes.Examples.AspNetCore.Storage;

public interface IStorageContext
{
    Task<TodoItem?> GetTodoItemAsync(Guid id);

    Task<IEnumerable<TodoItem>> GetAllTodoItemsAsync();

    Task<TodoItem> CreateTodoItemAsync(string todo, string createdBy, bool isPublic, bool isDone);

    Task UpdateTodoItemAsync(TodoItem item);
}
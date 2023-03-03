using Pipes.DependencyInjection;
using Pipes.Examples.AspNetCore.Models;
using Pipes.Examples.AspNetCore.Pipes.Pipeables;

namespace Pipes.Examples.AspNetCore.Pipes;

public static class RequestPipes
{
    public static class Todo
    {
        public static readonly ServicePipe<Guid, TodoItem?> Get = new()
        {
            typeof(GetTodoAsync),
            typeof(VerifyTodoPermissions),
            (IEnumerable<TodoItem> items) => items.SingleOrDefault()
        };

        public static readonly ServicePipe<IEnumerable<TodoItem>> GetAll = new()
        {
            typeof(GetAllTodoAsync),
            typeof(VerifyTodoPermissions)
        };

        public static readonly ServicePipe<CreateTodoItemRequest, TodoItem> Create = new()
        {
            typeof(CreateTodoAsync)
        };

        public static readonly ServicePipe<UpdateTodoItemRequest, TodoItem> Update = new()
        {
            typeof(UpdateTodoItemAsync)
        };
    }
}
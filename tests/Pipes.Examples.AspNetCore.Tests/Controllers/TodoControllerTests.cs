using Moq;
using Pipes.Examples.AspNetCore.Models;
using Pipes.Examples.AspNetCore.Storage;

namespace Pipes.Examples.AspNetCore.Tests.Controllers;

public class TodoControllerTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;

    public TodoControllerTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Test_GetTodoItemAsync()
    {
        var todoItem = new TodoItem
        {
            Id = Guid.NewGuid(),
            Todo = "Test Todo 1",
            CreatedBy = "test@test.de",
            IsDone = true,
            IsPublic = true
        };

        _factory.Mock<IStorageContext>(mock =>
        {
            mock.Setup(sc => sc.GetTodoItemAsync(It.IsAny<Guid>())).ReturnsAsync(todoItem);
        });

        using var client = _factory.CreateClient();

        var result = await client.GetFromJsonAsync<TodoItem>("api/todo/" + todoItem.Id);
        Assert.NotNull(result);
        Assert.Equal(todoItem.Id, result.Id);
        Assert.Equal(todoItem.Todo, result.Todo);

        _factory.Mock<IStorageContext>(mock =>
        {
            mock.Verify(sc => sc.GetTodoItemAsync(It.Is<Guid>(g => g == todoItem.Id)), Times.Once);
        });
    }

    [Fact]
    public async Task Test_GetAllTodoItemAsync()
    {
        var todoItems = new[]
        {
            new TodoItem
            {
                Id = Guid.NewGuid(),
                Todo = "Test Todo 1",
                CreatedBy = "test@test.de",
                IsDone = true,
                IsPublic = true
            }
        };

        _factory.Mock<IStorageContext>(mock =>
        {
            mock.Setup(sc => sc.GetAllTodoItemsAsync()).ReturnsAsync(todoItems);
        });

        using var client = _factory.CreateClient();

        var results = await client.GetFromJsonAsync<TodoItem[]>("api/todo");
        Assert.NotNull(results);
        Assert.Collection(results,
            result =>
            {
                Assert.NotNull(result);
                Assert.Equal(todoItems[0].Id, result.Id);
                Assert.Equal(todoItems[0].Todo, result.Todo);
            });

        _factory.Mock<IStorageContext>(mock => { mock.Verify(sc => sc.GetAllTodoItemsAsync(), Times.Once); });
    }

    [Fact]
    public async Task Test_CreateTodoItemAsync()
    {
        var request = new CreateTodoItemRequest
        {
            Todo = "Test Todo 1",
            IsDone = true,
            IsPublic = true
        };
        const string requestedBy = "test@test.de";

        _factory.Mock<IStorageContext>(mock =>
        {
            mock.Setup(sc =>
                    sc.CreateTodoItemAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync((string s1, string s2, bool b1, bool b2) => new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Todo = s1,
                    CreatedBy = s2,
                    IsPublic = b1,
                    IsDone = b2
                });
        });

        using var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("api/todo?requestedBy=" + requestedBy, request);
        var result = await response.Content.ReadFromJsonAsync<TodoItem>();
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(request.Todo, result.Todo);
        Assert.Equal(requestedBy, result.CreatedBy);

        _factory.Mock<IStorageContext>(mock =>
        {
            mock.Verify(
                sc => sc.CreateTodoItemAsync(It.Is<string>(s => s == request.Todo),
                    It.Is<string>(s => s == requestedBy), It.Is<bool>(b => b == request.IsPublic),
                    It.Is<bool>(b => b == request.IsDone)), Times.Once);
        });
    }

    [Fact]
    public async Task Test_UpdateTodoItemAsync()
    {
        var todoItem = new TodoItem
        {
            Id = Guid.NewGuid(),
            Todo = "Test Todo 1",
            CreatedBy = "test@test.de",
            IsDone = true,
            IsPublic = true
        };
        var request = new UpdateTodoItemRequest
        {
            Id = todoItem.Id,
            Todo = "Updated Test Todo 1",
            IsDone = true,
            IsPublic = true
        };
        const string requestedBy = "test@test.de";

        _factory.Mock<IStorageContext>(mock =>
        {
            mock.Setup(sc => sc.GetTodoItemAsync(It.IsAny<Guid>())).ReturnsAsync(todoItem);
        });

        using var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync($"api/todo/{todoItem.Id}?requestedBy=" + requestedBy, request);
        var result = await response.Content.ReadFromJsonAsync<TodoItem>();
        Assert.NotNull(result);
        Assert.Equal(todoItem.Id, result.Id);
        Assert.Equal(request.Todo, result.Todo);

        _factory.Mock<IStorageContext>(mock =>
        {
            mock.Verify(sc => sc.GetTodoItemAsync(It.Is<Guid>(g => g == todoItem.Id)), Times.Once);
            mock.Verify(sc => sc.UpdateTodoItemAsync(It.Is<TodoItem>(i => i.Id == todoItem.Id)), Times.Once);
        });
    }
}
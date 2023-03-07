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
            });
        
        _factory.Mock<IStorageContext>(mock =>
        {
            mock.Verify(sc => sc.GetAllTodoItemsAsync(), Times.Once);
        });
    }
}
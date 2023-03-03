using Pipes.DependencyInjection;
using Pipes.Examples.AspNetCore.Pipes;
using Pipes.Examples.AspNetCore.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IStorageContext, InMemoryStorage>();
builder.Services.AddScoped(RequestPipes.Todo.Get);

var app = builder.Build();

app.MapControllers();

app.Run();
using Pipes.AspNetCore;
using Pipes.DependencyInjection;
using Pipes.Examples.AspNetCore.Pipes;
using Pipes.Examples.AspNetCore.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IStorageContext, InMemoryStorage>();

builder.Services.AddScoped(RequestPipes.Todo.Get);
builder.Services.AddScoped(RequestPipes.Todo.GetAll);
builder.Services.AddScoped(RequestPipes.Todo.Update);
builder.Services.AddScoped(RequestPipes.Todo.Create);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

//TODO add pipe inheriting ServicePipe (no generic arguments)
app.UsePipes()
    .Add(RequestPipes.Todo.Get)
    .Add(RequestPipes.Todo.GetAll)
    .Add(RequestPipes.Todo.Update)
    .Add(RequestPipes.Todo.Create);

app.Run();
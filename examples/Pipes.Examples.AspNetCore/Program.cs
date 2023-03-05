using Pipes.AspNetCore;
using Pipes.Examples.AspNetCore.Pipes;
using Pipes.Examples.AspNetCore.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IStorageContext, InMemoryStorage>();

//TODO try overriding service lifetime
builder.Services.AddPipes()
    .Add(RequestPipes.Todo.Get)
    .Add(RequestPipes.Todo.GetAll)
    .Add(RequestPipes.Todo.Update)
    .Add(RequestPipes.Todo.Create);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UsePipes();

app.Run();
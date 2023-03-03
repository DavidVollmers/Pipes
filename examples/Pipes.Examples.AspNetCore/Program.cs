using Pipes.DependencyInjection;
using Pipes.Examples.AspNetCore.Pipes;
using Pipes.Examples.AspNetCore.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IStorageContext, InMemoryStorage>();

builder.Services.AddScoped(RequestPipes.Todo.Get);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
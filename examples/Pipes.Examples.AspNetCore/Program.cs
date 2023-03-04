using Pipes.DependencyInjection;
using Pipes.Examples.AspNetCore.Pipes;
using Pipes.Examples.AspNetCore.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IStorageContext, InMemoryStorage>();

//TODO allow scoped/singleton 
builder.Services.AddTransient(RequestPipes.Todo.Get);
builder.Services.AddTransient(RequestPipes.Todo.GetAll);
builder.Services.AddTransient(RequestPipes.Todo.Update);
builder.Services.AddTransient(RequestPipes.Todo.Create);

var app = builder.Build();

RequestPipes.Todo.Get.Activate(app.Services);
RequestPipes.Todo.GetAll.Activate(app.Services);
RequestPipes.Todo.Update.Activate(app.Services);
RequestPipes.Todo.Create.Activate(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
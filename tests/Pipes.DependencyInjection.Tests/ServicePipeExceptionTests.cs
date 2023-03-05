using Microsoft.Extensions.DependencyInjection;
using Pipes.DependencyInjection.Tests.Pipeables;

namespace Pipes.DependencyInjection.Tests;

public class ServicePipeExceptionTests
{
    [Fact]
    public void Test_Add_TypeIsNull()
    {
        var pipe = new ServicePipe();

        var exception = Assert.Throws<ArgumentNullException>(() => pipe.Add(null!));
        Assert.Equal("type", exception.ParamName);
    }

    [Fact]
    public void Test_Activate_ServiceProviderIsNull()
    {
        var pipe = new ServicePipe();

        var exception = Assert.Throws<ArgumentNullException>(() => pipe.Activate(null!));
        Assert.Equal("serviceProvider", exception.ParamName);
    }

    [Fact]
    public void Test_Activate_AlreadyActivated()
    {
        var pipe = new ServicePipe();

        var serviceProvider = new Mock<IServiceProvider>();

        pipe.Activate(serviceProvider.Object);

        var exception = Assert.Throws<InvalidOperationException>(() => pipe.Activate(serviceProvider.Object));
        Assert.Equal("Service pipe already activated. Use .Reset() before activating it again.", exception.Message);
    }

    [Fact]
    public void Test_Execute_PipeWasNotActivated()
    {
        var pipe = new ServicePipe
        {
            typeof(ServicePipeable)
        };

        var exception = Assert.Throws<InvalidOperationException>(() => pipe.Execute());
        Assert.Equal("Pipe was not activated before execution. Please use .Activate() before executing a pipe.",
            exception.Message);
    }

    [Fact]
    public void Test_TypeMustBeAssignableToIPipeable()
    {
        var pipe = new ServicePipe();

        var exception = Assert.Throws<Exception>(() => pipe.Add(typeof(ServicePipe<,>)));
        Assert.Equal("Type must be assignable to IPipeable.", exception.Message);
    }

    [Fact]
    public void Test_PipeableServiceAlreadyActivated()
    {
        var pipe = new ServicePipe
        {
            typeof(ServicePipeable)
        };
        
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(pipe);
        serviceCollection.AddSingleton<CounterService>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        pipe.Activate(serviceProvider);

        var pipeable = pipe.First<IPipeableService>();

        var exception = Assert.Throws<InvalidOperationException>(() => pipeable.Activate(serviceProvider, ServiceInjection.OnActivation));
        Assert.Equal("Pipeable service already activated. Use .Reset() before activating it again.", exception.Message);
    }

    [Fact]
    public void Test_ServiceLifetime()
    {
        var pipe = new ServicePipe
        {
            typeof(ServicePipeable)
        };
        
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<CounterService>();
        
        serviceCollection.AddTransient(pipe);
        Assert.Equal(ServiceLifetime.Transient, pipe.ServiceLifetime);
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        pipe.Activate(serviceProvider);

        var pipeable = pipe.First<IPipeableService>();
        Assert.Equal(ServiceLifetime.Transient, pipeable.ServiceLifetime);
    }
}
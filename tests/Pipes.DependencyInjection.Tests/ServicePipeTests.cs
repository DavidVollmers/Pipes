using Microsoft.Extensions.DependencyInjection;
using Pipes.DependencyInjection.Tests.Pipeables;

namespace Pipes.DependencyInjection.Tests;

public class ServicePipeTests
{
    [Fact]
    public void Test_Execute_NoPipeables()
    {
        var pipe = new ServicePipe();

        pipe.Execute();

        Assert.Null(pipe.Output);
    }

    [Fact]
    public async Task Test_ExecuteAsync_NoPipeables()
    {
        var pipe = new ServicePipe();

        await pipe.ExecuteAsync();

        Assert.Null(pipe.Output);
    }
    
    [Fact]
    public void Test_Execute_WithOutput_NoPipeables()
    {
        var pipe = new ServicePipe<object?>();

        var output =pipe.Execute();
        Assert.Null(output);
        
        Assert.Null(pipe.Output);
    }

    [Fact]
    public async Task Test_ExecuteAsync_WithOutput_NoPipeables()
    {
        var pipe = new ServicePipe<object?>();

        var output = await pipe.ExecuteAsync();
        Assert.Null(output);
        
        Assert.Null(pipe.Output);
    }

    [Fact]
    public void Test_Add()
    {
        var pipe = new ServicePipe();

        pipe.Add(typeof(ServicePipeable));
    }

    [Fact]
    public void Test_Dispose()
    {
        var pipe = new ServicePipe();

        var serviceScope = new Mock<IServiceScope>();

        var serviceScopeFactory = new Mock<IServiceScopeFactory>();
        serviceScopeFactory.Setup(ssf => ssf.CreateScope()).Returns(serviceScope.Object);

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(It.IsAny<Type>())).Returns(serviceScopeFactory.Object);

        pipe.Activate(serviceProvider.Object);

        pipe.Dispose();

        serviceScope.Verify(ss => ss.Dispose(), Times.Once);
    }

    [Fact]
    public void Test_Reset()
    {
        var pipe = new ServicePipe();

        var serviceScope = new Mock<IServiceScope>();

        var serviceScopeFactory = new Mock<IServiceScopeFactory>();
        serviceScopeFactory.Setup(ssf => ssf.CreateScope()).Returns(serviceScope.Object);

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(It.IsAny<Type>())).Returns(serviceScopeFactory.Object);

        pipe.Activate(serviceProvider.Object);

        pipe.Reset();

        serviceScope.Verify(ss => ss.Dispose(), Times.Once);
    }

    [Fact]
    public void Test_Activate_ServiceInjectionOnActivation()
    {
        var pipe = new ServicePipe
        {
            typeof(ServicePipeable)
        };

        var scopeServiceProvider = new Mock<IServiceProvider>();
        scopeServiceProvider.Setup(sp => sp.GetService(It.Is<Type>(t => t == typeof(ServicePipeable))))
            .Returns(new ServicePipeable(new CounterService()));

        var serviceScope = new Mock<IServiceScope>();
        serviceScope.Setup(ss => ss.ServiceProvider).Returns(scopeServiceProvider.Object);

        var serviceScopeFactory = new Mock<IServiceScopeFactory>();
        serviceScopeFactory.Setup(ssf => ssf.CreateScope()).Returns(serviceScope.Object);

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(It.IsAny<Type>())).Returns(serviceScopeFactory.Object);

        pipe.Activate(serviceProvider.Object);

        scopeServiceProvider.Verify(sp => sp.GetService(It.Is<Type>(t => t == typeof(ServicePipeable))), Times.Once);
    }

    [Fact]
    public void Test_Activate_ServiceInjectionOnInput()
    {
        var pipe = new ServicePipe(ServiceInjection.OnInput)
        {
            typeof(ServicePipeable)
        };

        var scopeServiceProvider = new Mock<IServiceProvider>();
        scopeServiceProvider.Setup(sp => sp.GetService(It.Is<Type>(t => t == typeof(ServicePipeable))))
            .Returns(new ServicePipeable(new CounterService()));

        var serviceScope = new Mock<IServiceScope>();
        serviceScope.Setup(ss => ss.ServiceProvider).Returns(scopeServiceProvider.Object);

        var serviceScopeFactory = new Mock<IServiceScopeFactory>();
        serviceScopeFactory.Setup(ssf => ssf.CreateScope()).Returns(serviceScope.Object);

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(It.IsAny<Type>())).Returns(serviceScopeFactory.Object);

        pipe.Activate(serviceProvider.Object);

        scopeServiceProvider.Verify(sp => sp.GetService(It.Is<Type>(t => t == typeof(ServicePipeable))), Times.Never);

        pipe.Execute();

        scopeServiceProvider.Verify(sp => sp.GetService(It.Is<Type>(t => t == typeof(ServicePipeable))), Times.Once);
    }
}
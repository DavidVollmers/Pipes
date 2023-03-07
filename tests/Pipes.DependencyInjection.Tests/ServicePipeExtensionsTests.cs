using Microsoft.Extensions.DependencyInjection;
using Pipes.DependencyInjection.Tests.Pipeables;

namespace Pipes.DependencyInjection.Tests;

public class ServicePipeExtensionsTests
{
    [Fact]
    public void Test_EnsureScopeActivation_ServicePipeIsNull()
    {
        var exception =
            Assert.Throws<ArgumentNullException>(() => ServicePipeExtensions.EnsureScopeActivation(null!, null!));
        Assert.Equal("servicePipe", exception.ParamName);
    }
    
    [Fact]
    public void Test_EnsureScopeActivation_ServiceProviderIsNull()
    {
        var servicePipe = new ServicePipe();
        
        var exception =
            // ReSharper disable once InvokeAsExtensionMethod
            Assert.Throws<ArgumentNullException>(() => ServicePipeExtensions.EnsureScopeActivation(servicePipe, null!));
        Assert.Equal("serviceProvider", exception.ParamName);
    }

    [Fact]
    public void Test_EnsureScopeActivation_ServicePipeNotActivated()
    {
        var servicePipe = new ServicePipe();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(servicePipe);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        servicePipe.EnsureScopeActivation(serviceProvider);
        
        Assert.True(servicePipe.Activated);
    }

    [Fact]
    public void Test_EnsureScopeActivation_ServicePipeableNotScoped()
    {
        var servicePipe = new ServicePipe
        {
            typeof(ServicePipeable)
        };

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(servicePipe);
        serviceCollection.AddSingleton<CounterService>();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        servicePipe.Activate(serviceProvider);
        
        servicePipe.EnsureScopeActivation(serviceProvider);
        
        Assert.True(servicePipe.Activated);
    }

    [Fact]
    public void Test_EnsureScopeActivation()
    {
        var servicePipe = new ServicePipe
        {
            typeof(ServicePipeable)
        };

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(servicePipe);
        serviceCollection.AddSingleton<CounterService>();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        servicePipe.Activate(serviceProvider);
        
        servicePipe.First<IPipeableService>().Reset();

        servicePipe.EnsureScopeActivation(serviceProvider);
        
        Assert.True(servicePipe.Activated);
    }

    [Fact]
    public void Test_EnsureScopeReset_ServicePipeIsNull()
    {
        var exception =
            Assert.Throws<ArgumentNullException>(() => ServicePipeExtensions.EnsureScopeReset(null!));
        Assert.Equal("servicePipe", exception.ParamName);
    }

    [Fact]
    public void Test_EnsureScopeReset_ServicePipeIsScoped()
    {
        var servicePipe = new ServicePipe();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(servicePipe);
        
        var serviceProvider = serviceCollection.BuildServiceProvider();

        servicePipe.Activate(serviceProvider);
        
        servicePipe.EnsureScopeReset();
        
        Assert.False(servicePipe.Activated);
    }

    [Fact]
    public void Test_EnsureScopeReset_ServicePipeableNotScoped()
    {
        var servicePipe = new ServicePipe
        {
            typeof(ServicePipeable)
        };

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(servicePipe);
        serviceCollection.AddSingleton<CounterService>();
        
        servicePipe.EnsureScopeReset();
        
        Assert.False(servicePipe.Activated);
    }

    [Fact]
    public void Test_EnsureScopeReset()
    {
        var servicePipe = new ServicePipe
        {
            typeof(ServicePipeable)
        };

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(servicePipe);
        serviceCollection.AddSingleton<CounterService>();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        servicePipe.First<IPipeableService>().Activate(serviceProvider, ServiceInjection.OnActivation);

        servicePipe.EnsureScopeReset();
        
        Assert.False(servicePipe.Activated);
    }
}
using Microsoft.Extensions.DependencyInjection;
using Pipes.DependencyInjection.Caching;
using Pipes.DependencyInjection.Tests.Pipeables;
using Pipes.Input;

namespace Pipes.DependencyInjection.Tests;

public class ServicePipeIntegrationTests
{
    private static readonly ServicePipe<int> StaticServicePipe = new()
    {
        typeof(ServicePipeable)
    };

    [Fact]
    public void Test_StaticServicePipe()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(StaticServicePipe);
        serviceCollection.AddSingleton<CounterService>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        StaticServicePipe.Activate(serviceProvider);

        var counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(0, counter.Value);

        var result = StaticServicePipe.Execute();
        Assert.Equal(1, result);
        Assert.Equal(1, StaticServicePipe.Output);

        counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(1, counter.Value);

        result = StaticServicePipe.Execute();
        Assert.Equal(2, result);
        Assert.Equal(2, StaticServicePipe.Output);

        counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(2, counter.Value);

        StaticServicePipe.Reset();
        Assert.Equal(0, StaticServicePipe.Output);

        counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(2, counter.Value);
    }

    [Fact]
    public void Test_AsyncServicePipe()
    {
        var servicePipe = new ServicePipe<int>
        {
            typeof(AsyncServicePipeable)
        };
        
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(servicePipe);
        serviceCollection.AddSingleton<CounterService>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        servicePipe.Activate(serviceProvider);

        var counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(0, counter.Value);

        var result = servicePipe.Execute();
        Assert.Equal(1, result);
        Assert.Equal(1, servicePipe.Output);

        counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(1, counter.Value);

        result = servicePipe.Execute();
        Assert.Equal(2, result);
        Assert.Equal(2, servicePipe.Output);

        counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(2, counter.Value);

        servicePipe.Reset();
        Assert.Equal(0, servicePipe.Output);

        counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(2, counter.Value);
    }

    [Fact]
    public void Test_CacheOutput()
    {
        var servicePipe = new ServicePipe<int>
        {
            Cache.Output<ServicePipeable>()
        };
        
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(servicePipe);
        serviceCollection.AddSingleton<CounterService>();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();

        servicePipe.Activate(serviceProvider);

        var counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(0, counter.Value);

        var result = servicePipe.Execute();
        Assert.Equal(1, result);
        Assert.Equal(1, servicePipe.Output);

        counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(1, counter.Value);

        result = servicePipe.Execute();
        Assert.Equal(1, result);
        Assert.Equal(1, servicePipe.Output);

        counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(1, counter.Value);

        servicePipe.Reset();
        Assert.Equal(0, servicePipe.Output);

        counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(1, counter.Value);
    }

    [Fact]
    public void Test_CacheOutput_Scoped()
    {
        var servicePipe = new ServicePipe<int>
        {
            Cache.Output<ServicePipeable>()
        };
        
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient(servicePipe);
        serviceCollection.AddSingleton<CounterService>();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var scope = serviceProvider.CreateScope();

        servicePipe.Activate(scope.ServiceProvider);

        var counter = scope.ServiceProvider.GetRequiredService<CounterService>();
        Assert.Equal(0, counter.Value);

        var result = servicePipe.Execute();
        Assert.Equal(1, result);
        Assert.Equal(1, servicePipe.Output);

        counter = scope.ServiceProvider.GetRequiredService<CounterService>();
        Assert.Equal(1, counter.Value);

        result = servicePipe.Execute();
        Assert.Equal(1, result);
        Assert.Equal(1, servicePipe.Output);

        counter = scope.ServiceProvider.GetRequiredService<CounterService>();
        Assert.Equal(1, counter.Value);

        scope.Dispose();
        
        servicePipe.Reset();
        Assert.Equal(0, servicePipe.Output);
        
        servicePipe.Activate(serviceProvider);
        
        result = servicePipe.Execute();
        Assert.Equal(2, result);
        Assert.Equal(2, servicePipe.Output);
        
        counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(2, counter.Value);
    }
}
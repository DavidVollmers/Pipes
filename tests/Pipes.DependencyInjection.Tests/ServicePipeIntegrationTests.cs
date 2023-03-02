using Microsoft.Extensions.DependencyInjection;
using Pipes.DependencyInjection.Tests.Pipeables;
using Pipes.Input;

namespace Pipes.DependencyInjection.Tests;

public class ServicePipeIntegrationTests
{
    private static readonly ServicePipe<object, int> StaticServicePipe = new()
    {
        typeof(ServicePipeable)
    };

    [Fact]
    public void Test_StaticServicePipe()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<CounterService>();
        serviceCollection.AddTransient<ServicePipeable>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        StaticServicePipe.Activate(serviceProvider);

        var counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(0, counter.Value);

        var result = StaticServicePipe.Execute(PipeInput.Empty);
        Assert.Equal(1, result);
        
        counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(1, counter.Value);
        
        result = StaticServicePipe.Execute(PipeInput.Empty);
        Assert.Equal(2, result);
        
        counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(2, counter.Value);
        
        StaticServicePipe.Reset();
        
        counter = serviceProvider.GetRequiredService<CounterService>();
        Assert.Equal(2, counter.Value);
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Pipes.AspNetCore.Tests;

public class IntegrationTests
{
    [Fact]
    public async Task Test_AddAndUsePipes()
    {
        var servicePipeSingleton = new ServicePipe();
        var servicePipeScoped = new ServicePipe();
        var servicePipeTransient = new ServicePipe();
        
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddPipes()
            .AddSingleton(servicePipeSingleton)
            .AddScoped(servicePipeScoped)
            .AddTransient(servicePipeTransient);
        
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var applicationBuilder = new ApplicationBuilder(serviceProvider);

        applicationBuilder.UsePipes();
        
        Assert.True(servicePipeSingleton.Activated);
        Assert.Equal(ServiceLifetime.Singleton, servicePipeSingleton.ServiceLifetime);
        
        Assert.False(servicePipeScoped.Activated);
        Assert.Equal(ServiceLifetime.Scoped, servicePipeScoped.ServiceLifetime);
        
        Assert.False(servicePipeTransient.Activated);
        Assert.Equal(ServiceLifetime.Transient, servicePipeTransient.ServiceLifetime);
    }
}
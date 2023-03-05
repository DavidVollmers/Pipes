using Microsoft.Extensions.DependencyInjection;
using Pipes.DependencyInjection.Tests.Pipeables;

namespace Pipes.DependencyInjection.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void Test_Add_ServiceCollectionIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ServiceCollectionExtensions.Add<object, object>(null!, null!, ServiceLifetime.Transient));
        Assert.Equal("serviceCollection", exception.ParamName);
    }

    [Fact]
    public void Test_Add_ServicePipeIsNull()
    {
        var serviceCollection = new Mock<IServiceCollection>();

        var exception = Assert.Throws<ArgumentNullException>(() =>
            // ReSharper disable once InvokeAsExtensionMethod
            ServiceCollectionExtensions.Add<object, object>(serviceCollection.Object, null!,
                ServiceLifetime.Transient));
        Assert.Equal("pipe", exception.ParamName);
    }

    [Theory]
    [InlineData(ServiceLifetime.Transient)]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Singleton)]
    public void Test_Add(ServiceLifetime serviceLifetime)
    {
        var servicePipe = new ServicePipe
        {
            typeof(ServicePipeable)
        };

        var serviceCollection = new Mock<IServiceCollection>();

        // ReSharper disable once InvokeAsExtensionMethod
        ServiceCollectionExtensions.Add(serviceCollection.Object, servicePipe, serviceLifetime);

        serviceCollection.Verify(sc => sc.Add(It.Is<ServiceDescriptor>(sd =>
            sd.ServiceType == typeof(ServicePipeable) && sd.ImplementationType == typeof(ServicePipeable) &&
            sd.Lifetime == serviceLifetime)), Times.Once);
    }

    [Fact]
    public void Test_Add_NoPipeableType()
    {
        var servicePipe = new ServicePipe
        {
            (object o) => o
        };

        var serviceCollection = new Mock<IServiceCollection>();

        // ReSharper disable once InvokeAsExtensionMethod
        ServiceCollectionExtensions.Add(serviceCollection.Object, servicePipe, ServiceLifetime.Transient);

        serviceCollection.Verify(sc => sc.Add(It.IsAny<ServiceDescriptor>()), Times.Once);
    }

    [Fact]
    public void Test_AddTransient()
    {
        var servicePipe = new ServicePipe
        {
            typeof(ServicePipeable)
        };

        var serviceCollection = new Mock<IServiceCollection>();

        // ReSharper disable once InvokeAsExtensionMethod
        ServiceCollectionExtensions.AddTransient(serviceCollection.Object, servicePipe);

        serviceCollection.Verify(sc => sc.Add(It.Is<ServiceDescriptor>(sd =>
            sd.ServiceType == typeof(ServicePipeable) && sd.ImplementationType == typeof(ServicePipeable) &&
            sd.Lifetime == ServiceLifetime.Transient)), Times.Once);
    }

    [Fact]
    public void Test_AddScoped()
    {
        var servicePipe = new ServicePipe
        {
            typeof(ServicePipeable)
        };

        var serviceCollection = new Mock<IServiceCollection>();

        // ReSharper disable once InvokeAsExtensionMethod
        ServiceCollectionExtensions.AddScoped(serviceCollection.Object, servicePipe);

        serviceCollection.Verify(sc => sc.Add(It.Is<ServiceDescriptor>(sd =>
            sd.ServiceType == typeof(ServicePipeable) && sd.ImplementationType == typeof(ServicePipeable) &&
            sd.Lifetime == ServiceLifetime.Scoped)), Times.Once);
    }

    [Fact]
    public void Test_AddSingleton()
    {
        var servicePipe = new ServicePipe
        {
            typeof(ServicePipeable)
        };

        var serviceCollection = new Mock<IServiceCollection>();

        // ReSharper disable once InvokeAsExtensionMethod
        ServiceCollectionExtensions.AddSingleton(serviceCollection.Object, servicePipe);

        serviceCollection.Verify(sc => sc.Add(It.Is<ServiceDescriptor>(sd =>
            sd.ServiceType == typeof(ServicePipeable) && sd.ImplementationType == typeof(ServicePipeable) &&
            sd.Lifetime == ServiceLifetime.Singleton)), Times.Once);
    }
}
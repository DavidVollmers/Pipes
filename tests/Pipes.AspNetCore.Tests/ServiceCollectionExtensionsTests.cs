using Microsoft.Extensions.DependencyInjection;

namespace Pipes.AspNetCore.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void Test_AddPipes_ServiceCollectionIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddPipes(null!));
        Assert.Equal("serviceCollection", exception.ParamName);
    }

    [Fact]
    public void Test_AddPipes()
    {
        var serviceCollection = new Mock<IServiceCollection>();

        // ReSharper disable once InvokeAsExtensionMethod
        ServiceCollectionExtensions.AddPipes(serviceCollection.Object);
        
        serviceCollection.Verify(sc => sc.Add(It.IsAny<ServiceDescriptor>()), Times.Once);
    }
}
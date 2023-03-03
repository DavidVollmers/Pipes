using Microsoft.Extensions.DependencyInjection;

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

        var serviceScope = new Mock<IServiceScope>();

        var serviceScopeFactory = new Mock<IServiceScopeFactory>();
        serviceScopeFactory.Setup(ssf => ssf.CreateScope()).Returns(serviceScope.Object);

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(It.IsAny<Type>())).Returns(serviceScopeFactory.Object);

        pipe.Activate(serviceProvider.Object);

        var exception = Assert.Throws<InvalidOperationException>(() => pipe.Activate(serviceProvider.Object));
        Assert.Equal("Service pipe already activated. Use .Reset() before activating it again.", exception.Message);

        serviceScopeFactory.Verify(ssf => ssf.CreateScope(), Times.Once);
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Pipes.AspNetCore.Tests;

public class ApplicationBuilderExtensionsTests
{
    [Fact]
    public void Test_UsePipes_ApplicationBuilderIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => ApplicationBuilderExtensions.UsePipes(null!));
        Assert.Equal("applicationBuilder", exception.ParamName);
    }

    [Fact]
    public void Test_UsePipes()
    {
        var applicationBuilder = new Mock<IApplicationBuilder>();

        // ReSharper disable once InvokeAsExtensionMethod
        ApplicationBuilderExtensions.UsePipes(applicationBuilder.Object);
        
        applicationBuilder.Verify(ab => ab.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()), Times.Once);
    }
}
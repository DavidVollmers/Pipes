namespace Pipes.Input.Tests;

public class TypeMapExtensionsTests
{
    [Fact]
    public void Test_Add_TypeMapping_TypeMapIsNull()
    {
        var mapping = new TypeMapping<string, int>(int.Parse);
        
        var exception = Assert.Throws<ArgumentNullException>(() => TypeMapExtensions.Add(null!, mapping));
        Assert.Equal("typeMap", exception.ParamName);
    }
    
    [Fact]
    public void Test_Add_TypeMapping()
    {
        var typeMap = new TypeMap<int>();
        var mapping = new TypeMapping<string, int>(int.Parse);

        // ReSharper disable once InvokeAsExtensionMethod
        TypeMapExtensions.Add(typeMap, mapping);

        // needed for coder coverage
        typeMap.First().Mapper("0");
    }
    
    [Fact]
    public void Test_Add_Delegate_TypeMapIsNull()
    {
        Func<string, int> mapper = null!;
        
        var exception = Assert.Throws<ArgumentNullException>(() => TypeMapExtensions.Add(null!, mapper));
        Assert.Equal("typeMap", exception.ParamName);
    }
    
    [Fact]
    public void Test_Add_Delegate_MapperIsNull()
    {
        var typeMap = new TypeMap<int>();
        Func<string, int> mapper = null!;
        
        // ReSharper disable once InvokeAsExtensionMethod
        var exception = Assert.Throws<ArgumentNullException>(() => TypeMapExtensions.Add(typeMap, mapper));
        Assert.Equal("mapper", exception.ParamName);
    }
    
    [Fact]
    public void Test_Add_Delegate()
    {
        var typeMap = new TypeMap<int>();
        Func<string, int> mapper = int.Parse;

        // ReSharper disable once InvokeAsExtensionMethod
        TypeMapExtensions.Add(typeMap, mapper);
    }
}
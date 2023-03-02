using System.Collections;

namespace Pipes.Input.Tests;

public class TypeMapTests
{
    [Fact]
    public void Test_AddAndGetEnumerator()
    {
        var typeMapping = new TypeMapping<string, int>(int.Parse);
        // ReSharper disable once UseObjectOrCollectionInitializer
        var typeMap = new TypeMap<int>();
        
        typeMap.Add(typeMapping);
        typeMap.Add(typeMapping);
        
        Assert.Collection(typeMap,
            result => Assert.Equal(typeMapping.Type, result.Type),
            result => Assert.Equal(typeMapping.Type, result.Type));
    }

    [Fact]
    public void Test_GetEnumerator_IEnumerableCast()
    {
        var typeMapping = new TypeMapping<string, int>(int.Parse);
        // ReSharper disable once UseObjectOrCollectionInitializer
        var typeMap = new TypeMap<int>();
        
        typeMap.Add(typeMapping);
        typeMap.Add(typeMapping);

        foreach (var result in (IEnumerable) typeMap)
        {
            Assert.Equal(typeof(string), ((dynamic) result).Type);
        }
    }
}
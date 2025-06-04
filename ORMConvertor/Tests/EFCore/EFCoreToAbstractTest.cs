using AbstractWrappers;
using EFCoreWrappers;
using Newtonsoft.Json;
using SampleData;

namespace Tests.EFCore;

public class EFCoreToAbstractTest
{
    [Fact]
    public void EFCoreToAbstractOverall()
    {
        // Using dummy builder because we need a concrete implementation,
        // however we will only be inspecting how the abstract representation is built
        AbstractEntityBuilder builder = new DummyEntityBuilder();
        var entityParser = new EFCoreEntityParser(builder);
        entityParser.Parse(CustomerSampleEFCore.Entity);

        Assert.Equal(JsonConvert.SerializeObject(CustomerSampleEFCore.Map), JsonConvert.SerializeObject(builder.EntityMap), ignoreLineEndingDifferences: true);
    }
}

using AbstractWrappers;
using EFCoreWrappers;
using Model;
using Tests.SampleData;

namespace Tests.EFCore;

public class AbstractToEFCoreTest
{
    [Fact]
    public void AbstractToEFCoreOverall()
    {
        var source = CustomerMapEFCore.Map;

        AbstractEntityBuilder builder = new EFCoreEntityBuilder
        {
            EntityMap = source
        };

        var results = builder.Build();
        var entityOutput = results.Single();

        Assert.Equal(ContentType.CSharp, entityOutput.ContentType);
        Assert.Equal(CustomerMapEFCore.Source, entityOutput.Content, ignoreLineEndingDifferences: true);
    }
}

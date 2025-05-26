using AbstractWrappers;
using DapperWrappers;
using Model;
using Tests.SampleData;

namespace Tests.Dapper;

public class AbstractToDapperTest
{
    [Fact]
    public void AbstractToDapperOverall()
    {
        var source = CustomerMapDapper.Map;

        AbstractEntityBuilder builder = new DapperEntityBuilder
        {
            EntityMap = source
        };

        var results = builder.Build();
        var entityOutput = results.Single();

        Assert.Equal(ContentType.CSharp, entityOutput.ContentType);
        Assert.Equal(CustomerMapDapper.Source, entityOutput.Content, ignoreLineEndingDifferences: true);
    }
}

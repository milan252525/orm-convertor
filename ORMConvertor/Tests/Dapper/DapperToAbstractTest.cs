using AbstractWrappers;
using DapperWrappers;
using Newtonsoft.Json;
using Tests.SampleData;

namespace Tests.Dapper;

public class DapperToAbstractTest
{
    [Fact]
    public void DapperToAbstractOverall() {
        var sourceCode = CustomerMapDapper.Source;

        AbstractEntityBuilder builder = new DapperEntityBuilder();
        var parser = new DapperEntityParser(builder);

        parser.Parse(sourceCode);

        Assert.Equal(JsonConvert.SerializeObject(CustomerMapDapper.Map), JsonConvert.SerializeObject(builder.EntityMap), ignoreLineEndingDifferences: true);
    }
}

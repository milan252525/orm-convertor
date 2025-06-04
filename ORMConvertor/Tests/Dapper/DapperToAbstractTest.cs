using AbstractWrappers;
using DapperWrappers;
using Newtonsoft.Json;
using SampleData;

namespace Tests.Dapper;

public class DapperToAbstractTest
{
    [Fact]
    public void DapperToAbstractOverall() {
        var sourceCode = CustomerSampleDapper.Entity;

        AbstractEntityBuilder builder = new DummyEntityBuilder();
        var parser = new DapperEntityParser(builder);

        parser.Parse(sourceCode);

        Assert.Equal(JsonConvert.SerializeObject(CustomerSampleDapper.Map), JsonConvert.SerializeObject(builder.EntityMap), ignoreLineEndingDifferences: true);
    }
}

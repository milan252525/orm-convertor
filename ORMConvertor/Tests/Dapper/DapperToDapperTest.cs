using AbstractWrappers;
using DapperWrappers;
using Tests.SampleData;

namespace Tests.Dapper;

public class DapperToDapperTest
{
    [Fact]
    public void DapperToDapperOverall()
    {
        AbstractEntityBuilder builder = new DapperEntityBuilder();
        var parser = new DapperEntityParser(builder);

        parser.Parse(CustomerMapDapper.Source);
        var result = builder.Build().Single();

        Assert.Equal(Model.ContentType.CSharp, result.ContentType);
        Assert.Equal(CustomerMapDapper.Source, result.Content.Trim());
    }
}

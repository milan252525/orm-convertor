using AbstractWrappers;
using DapperWrappers;
using Tests.SampleData;

namespace Tests.Dapper;

public class DapperToDapperTest
{
    [Fact]
    public void DapperToDapperOverall()
    {
        var source = """
            namespace DapperEntities;

            public class Customer
            {
            	public int CustomerID { get;set; }

            	public required string CustomerName { get;set; }

            	public DateTime AccountOpenedDate { get;set; }

            	public decimal? CreditLimit { get;set; }

            	public List<CustomerTransaction> Transactions { get;set; } = [];

            }
            """;

        AbstractEntityBuilder builder = new DapperEntityBuilder();
        var parser = new DapperEntityParser(builder);

        parser.Parse(source);
        var result = builder.Build();

        Assert.Equal(Model.ContentType.CSharp, result.ContentType);
        Assert.Equal(source, result.Content.Trim());
    }
}

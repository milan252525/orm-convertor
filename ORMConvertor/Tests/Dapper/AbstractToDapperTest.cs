using AbstractWrappers;
using DapperWrappers;
using Tests.SampleData;

namespace Tests.Dapper;

public class AbstractToDapperTest
{
    [Fact]
    public void AbstractToDapperOverall()
    {
        var source = CustomerMapEFCore.Map;

        AbstractEntityBuilder builder = new DapperEntityBuilder
        {
            EntityMap = source
        };

        var results = builder.Build();
        var entityOutput = results.Single();

        var expectedResult = """
            namespace EFCoreEntities;

            public class Customer
            {
            	public int CustomerID { get;set; }

            	public required string CustomerName { get;set; }

            	public DateTime AccountOpenedDate { get;set; }

            	public decimal? CreditLimit { get;set; }

            	public List<CustomerTransaction> Transactions { get;set; } = [];

            }
            """;

        Assert.Equal(Model.ContentType.CSharp, entityOutput.ContentType);
        Assert.Equal(expectedResult, entityOutput.Content.Trim());
    }
}

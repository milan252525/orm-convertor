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

        var result = builder.Build();

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

        Assert.Equal(Model.ContentType.CSharp, result.ContentType);
        Assert.Equal(expectedResult, result.Content.Trim());
    }
}

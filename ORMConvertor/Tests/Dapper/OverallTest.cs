using AbstractWrappers;
using DapperWrappers;
using Tests.SampleData;

namespace Tests.Dapper;
public class OverallTest
{
    [Fact]
    public void AbstractToDapper()
    {
        var source = CustomerMap.Map;

        AbstractEntityBuilder builder = new DapperEntityBuilder
        {
            EntityMap = source
        };

        var result = builder.Build();

        var expectedResult = """
            namespace EFCoreEntities;

            public class Person
            {
            	public int CustomerID { get;set; }
            	public required int CustomerName { get;set; }
            	public DateTime AccountOpenedDate { get;set; }
            	public decimal? CreditLimit { get;set; }
            	public List<CustomerTransaction> Transactions { get;set; }
            }
            """;

        Assert.Equal(Model.ContentType.CSharp, result.ContentType);
        Assert.Equal(expectedResult, result.Content.Trim());
    }
}

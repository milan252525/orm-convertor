using AbstractWrappers;
using DapperWrappers;

namespace Tests;

public class QueryBuilderTest
{
    [Fact]
    public void QueryBuilder()
    {
        AbstractQueryBuilder builder = new DapperSQLQueryBuilder();

        builder.Project("Sales.Customer", "CustomerName", "name");
        builder.From("Sales.Customer");
        builder.Select("name = 'Joe'");
        builder.OrderBy("Sales.Customer", ["name"], desc: true);
        var sql = builder.Build();

        string expected = """
        SELECT Sales.Customer.CustomerName AS name
        FROM Sales.Customer
        WHERE name = 'Joe'
        ORDER BY Sales.Customer.name DESC

        """;
        Assert.Equal(expected, sql);
    }
}

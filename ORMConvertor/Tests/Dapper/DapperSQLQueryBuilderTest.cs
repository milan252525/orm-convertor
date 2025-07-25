﻿using AbstractWrappers;
using DapperWrappers;
using Model.QueryInstructions.Enums;

namespace Tests.Dapper;

public class DapperSQLQueryBuilderTest
{
    [Fact]
    public void SelectWithAllQueryInstructions()
    {
        AbstractQueryBuilder builder = new DapperSqlQueryBuilder();

        builder.Push();
        builder.Project("c", "CustomerName", "Name");
        builder.Project("ord", "Id", function: "COUNT", alias: "OrderCount");
        builder.Project("ord", "TotalPrice", function: "SUM", alias: "TotalSpent");
        builder.From("Sales.Customer", alias: "c");
        builder.Join(JoinKind.Inner, "c", "Sales.Orders", "Id", "CustomerId", rightTableAlias: "ord");
        builder.Select("c", "Id", null, BooleanOperator.NotEqual, null, null, "25");
        builder.Select("ord", "TotalPrice", null, BooleanOperator.GreaterThanOrEqual, "c", "MaxOrderLimit", null);
        builder.OrderBy(null, "Name", asc: false);
        builder.OrderBy(null, "TotalSpent", asc: true);
        builder.GroupBy("c", "CustomerName");
        builder.Having("ord", "TotalPrice", null, "SUM", BooleanOperator.GreaterThan, null, null, "1000", null);
        builder.Pop();
        
        var sql = builder.Build().First().Content;

        string expected = """"
        public List<Customer> Query() 
        {
            return connection.Query<Customer>(
                """
                SELECT c.CustomerName AS Name, COUNT(ord.Id) AS OrderCount, SUM(ord.TotalPrice) AS TotalSpent
                FROM Sales.Customer AS c
                INNER JOIN Sales.Orders ord ON c.Id = ord.CustomerId
                WHERE c.Id <> 25
                	AND ord.TotalPrice >= c.MaxOrderLimit
                GROUP BY c.CustomerName
                HAVING SUM(ord.TotalPrice) > 1000
                ORDER BY Name DESC, TotalSpent ASC
                """,    
            ).ToList();
        }
        """";

        Assert.Equal(expected, sql, ignoreAllWhiteSpace: true, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void SetOperationOnTwoSelects()
    {
        AbstractQueryBuilder builder = new DapperSqlQueryBuilder();

        builder.Push();
        builder.Project("c", "CustomerName", "Name");
        builder.From("Sales.Customer", alias: "c");
        builder.Pop();
        builder.SetOperation(SetOperationType.Union);
        builder.Push();
        builder.Project("c", "CustomerName", "Name");
        builder.From("Sales.Customer", alias: "c");
        builder.Pop();

        var sql = builder.Build().First().Content;

        string expected = """"
        public List<Customer> Query() 
        {
            return connection.Query<Customer>(
                """
                SELECT c.CustomerName AS Name
                FROM Sales.Customer AS c
                
                UNION

                SELECT c.CustomerName AS Name
                FROM Sales.Customer AS c
                """,    
            ).ToList();
        }
        """";

        Assert.Equal(expected, sql, ignoreAllWhiteSpace: true, ignoreLineEndingDifferences: true);
    }
}

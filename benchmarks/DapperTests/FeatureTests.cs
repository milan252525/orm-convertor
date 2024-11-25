namespace DapperTests;

using System.Data.SqlClient;
using Common;
using Dapper;
using DapperTests.Entities;

[Collection("Dapper")]
public class FeatureTests()
{
    // Constructor is run before every test
    private readonly SqlConnection connection = new(DatabaseConfig.MSSQLConnectionString);

    [Fact]
    public void InitialTest()
    {
        int count = connection.Query(
            "SELECT * FROM [WideWorldImporters].[Purchasing].[PurchaseOrders]"
        ).Count();
        Assert.Equal(2074, count);
    }

    [Fact]
    public void Select_Basic()
    {
        var order = connection.Query<PurchaseOrder>(
            "SELECT * FROM [WideWorldImporters].[Purchasing].[PurchaseOrders] WHERE [PurchaseOrderId] = 25"
        ).Single();
        
        Assert.Equal(25, order.PurchaseOrderID);
        Assert.Equal(12, order.SupplierID);
        Assert.Equal(new DateTime(2013, 1, 5), order.OrderDate);
    }
}

namespace Dapper.Features;

using System.Data;
using System.Data.SqlClient;
using Common;
using Dapper;
using Dapper.Entities;

[Collection("Dapper")]
public class FeatureTests
{
    // Constructor is run before every test
    private readonly SqlConnection connection = new(DatabaseConfig.MSSQLConnectionString);

    public FeatureTests()
    {
        /// Stored procedure result has spaces in column names which we can't map onto C# properties
        SqlMapper.SetTypeMap(
            typeof(PurchaseOrderUpdate),
        new CustomPropertyTypeMap(
                typeof(PurchaseOrderUpdate),
                (type, columnName) => type.GetProperty(columnName.Replace(" ", "").Replace("WWI", ""))!
            )
        );
    }

    [Fact]
    public void A1_EntityIdenticalToTable()
    {
        var order = connection.QuerySingle<PurchaseOrder>(
            "SELECT * FROM WideWorldImporters.Purchasing.PurchaseOrders WHERE PurchaseOrderId = @PurchaseOrderId",
            new { PurchaseOrderId = 25 }
        );

        Assert.Equal(25, order.PurchaseOrderID);
        Assert.Equal(12, order.SupplierID);
        Assert.Equal(new DateTime(2013, 1, 5), order.OrderDate);
        Assert.Equal(7, order.DeliveryMethodID);
        Assert.Equal(2, order.ContactPersonID);
        Assert.Equal(new DateTime(2013, 1, 25), order.ExpectedDeliveryDate);
        Assert.Equal("237408032", order.SupplierReference);
        Assert.True(order.IsOrderFinalized);
        Assert.Null(order.Comments);
        Assert.Null(order.InternalComments);
        Assert.Equal(14, order.LastEditedBy);
        Assert.Equal(new DateTime(2013, 1, 7, 7, 0, 0), order.LastEditedWhen);
    }

    [Fact]

    public void A2_LimitedEntity()
    {
        var contactInfo = connection.QuerySingle<SupplierContactInfo>(
            "SELECT SupplierID, SupplierName, PhoneNumber, FaxNumber, WebsiteURL, ValidFrom, ValidTo FROM WideWorldImporters.Purchasing.Suppliers WHERE SupplierID = @SupplierID",
            new { SupplierID = 10 }
        );

        Assert.Equal(10, contactInfo.SupplierID);
        Assert.Equal("Northwind Electric Cars", contactInfo.SupplierName);
        Assert.Equal("(201) 555-0105", contactInfo.PhoneNumber);
        Assert.Equal("(201) 555-0104", contactInfo.FaxNumber);
        Assert.Equal("http://www.northwindelectriccars.com", contactInfo.WebsiteURL);
        Assert.Equal(new DateTime(2013, 1, 1, 0, 5, 0), contactInfo.ValidFrom);
        Assert.Equal(DateTime.MaxValue, contactInfo.ValidTo);
    }

    [Fact]
    public void A3_MultipleEntitiesFromOneResult()
    {
        (var contactInfo, var bankAccount) = connection.Query<SupplierContactInfo, SupplierBankAccount, (SupplierContactInfo, SupplierBankAccount)>(
            """
                SELECT 
                    SupplierId, SupplierName, PhoneNumber, FaxNumber, WebsiteURL, ValidFrom, ValidTo, 
                    SupplierId, BankAccountName, BankAccountBranch, BankAccountCode, BankAccountNumber, BankInternationalCode 
                FROM WideWorldImporters.Purchasing.Suppliers WHERE SupplierID = @SupplierID
                """,
            (contactInfo, bankAccount) => (contactInfo, bankAccount),
            new { SupplierID = 10 },
            splitOn: nameof(SupplierBankAccount.SupplierID)
        ).First();

        var expectedContactInfo = new SupplierContactInfo
        {
            SupplierID = 10,
            SupplierName = "Northwind Electric Cars",
            PhoneNumber = "(201) 555-0105",
            FaxNumber = "(201) 555-0104",
            WebsiteURL = "http://www.northwindelectriccars.com",
            ValidFrom = new DateTime(2013, 1, 1, 0, 5, 0),
            ValidTo = DateTime.MaxValue
        };

        Assert.Equal(expectedContactInfo, contactInfo);

        var expectedBankAccount = new SupplierBankAccount
        {
            SupplierID = 10,
            BankAccountName = "Northwind Electric Cars",
            BankAccountBranch = "Woodgrove Bank Crandon Lakes",
            BankAccountCode = "325447",
            BankAccountNumber = "3258786987",
            BankInternationalCode = "36214"
        };

        Assert.Equal(expectedBankAccount, bankAccount);
    }

    [Fact]
    public void A4_StoredProcedureToEntity()
    {
        var orders = connection.Query<PurchaseOrderUpdate>(
            "WideWorldImporters.Integration.GetOrderUpdates",
            new { LastCutoff = new DateTime(2014, 1, 1), NewCutoff = new DateTime(2015, 1, 1) },
            commandType: CommandType.StoredProcedure
        ).ToList();

        Assert.Equal(66741, orders.Count);
        Assert.True(orders.All(o => o.Quantity > 0));
        Assert.True(orders.All(o => o.TaxRate > 0));
        Assert.True(orders.All(o => o.UnitPrice > 0));

        var expectedFirstUpdate = new PurchaseOrderUpdate()
        {
            OrderID = 1219,
            Description = "\"The Gu\" red shirt XML tag t-shirt (White) XS",
            Quantity = 96,
            UnitPrice = 18.00m,
            TaxRate = 15m,
            TotalIncludingTax = 1987.2m
        };

        Assert.Equal(expectedFirstUpdate, orders.First());
    }

    [Fact]
    public void B1_SelectionOverIndexedColumn()
    {
        int orderId = 26866;

        var orderLines = connection.Query<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines WHERE OrderID = @OrderID",
            new { OrderID = orderId }
        ).ToList();

        Assert.Equal(5, orderLines.Count);
        Assert.True(orderLines.All(ol => ol.OrderID == orderId));

        var expectedFirstOrderLine = new OrderLine
        {
            OrderLineID = 85261,
            OrderID = orderId,
            StockItemID = 141,
            Description = "Furry animal socks (Pink) XL",
            PackageTypeID = 10,
            Quantity = 48,
            UnitPrice = 5m,
            TaxRate = 15m,
            PickedQuantity = 48,
            PickingCompletedWhen = new DateTime(2014, 5, 14, 11, 0, 0),
            LastEditedBy = 19,
            LastEditedWhen = new DateTime(2014, 5, 14, 11, 0, 0)
        };

        Assert.Equal(expectedFirstOrderLine, orderLines.First(ol => ol.OrderLineID == 85261));
    }

    [Fact]
    public void B2_SelectionOverNonIndexedColumn()
    {
        decimal unitPrice = 25m;

        var orderLines = connection.Query<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines WHERE UnitPrice = @UnitPrice",
            new { UnitPrice = unitPrice }
        ).ToList();

        Assert.Equal(13553, orderLines.Count);
        Assert.True(orderLines.All(ol => ol.UnitPrice == unitPrice));
    }

    [Fact]
    public void B3_RangeQuery()
    {
        var from = new DateTime(2014, 12, 20);
        var to = new DateTime(2014, 12, 31);
        var orderLines = connection.Query<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines WHERE PickingCompletedWhen BETWEEN @Start AND @End",
            new { Start = from, End = to }
        ).ToList();

        Assert.Equal(1883, orderLines.Count);
        Assert.True(orderLines.All(ol => ol.PickingCompletedWhen >= from && ol.PickingCompletedWhen <= to));
    }

    [Fact]
    public void B4_InQuery()
    {
        var orderIds = new[] { 1, 10, 100, 1000, 10000 };

        var orderLines = connection.Query<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines WHERE OrderID IN @OrderIds",
            new { OrderIds = orderIds }
        ).ToList();

        Assert.Equal(15, orderLines.Count);
        Assert.True(orderLines.All(ol => orderIds.Contains(ol.OrderID)));
    }

    [Fact]
    public void B5_TextSearch()
    {
        string text = "C++";

        var orderLines = connection.Query<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines WHERE Description LIKE @Text",
            new { Text = $"%{text}%" }
        ).ToList();

        Assert.Equal(2098, orderLines.Count);
        Assert.True(orderLines.All(ol => ol.Description.Contains(text)));
    }

    [Fact]
    public void B6_PagingQuery()
    {
        int skip = 1000;
        int take = 50;

        var orderLines = connection.Query<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines ORDER BY OrderLineID OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY",
            new { Skip = skip, Take = take }
        ).ToList();

        Assert.Equal(50, orderLines.Count);
        Assert.Equal(Enumerable.Range(skip + 1, take), orderLines.Select(ol => ol.OrderLineID));
    }

    [Fact]
    public void C1_AggregationCount()
    {
        var taxRates = connection.Query<(decimal, int)>(
            """
                SELECT TaxRate, COUNT(TaxRate) as Count 
                FROM WideWorldImporters.Sales.OrderLines 
                GROUP BY TaxRate 
                ORDER BY Count DESC
            """
        ).ToDictionary(x => x.Item1, x => x.Item2);

        Assert.Equal(2, taxRates.Count);
        Assert.Equal(230376, taxRates[15m]);
        Assert.Equal(1036, taxRates[10m]);
    }

    [Fact]
    public void C2_AggregationMax()
    {
        var maxUnitPrice = connection.Query<decimal>(
            "SELECT MAX(UnitPrice) FROM WideWorldImporters.Sales.OrderLines"
        ).Single();

        Assert.Equal(1899m, maxUnitPrice);
    }

    [Fact]
    public void C3_AggregationSum()
    {
        var totalSales = connection.Query<decimal>(
            "SELECT SUM(Quantity * UnitPrice) FROM WideWorldImporters.Sales.OrderLines"
        ).Single();

        Assert.Equal(177634276.4m, totalSales);
    }

    [Fact]
    public void D1_1ToNRelationship()
    {
        string sql = """
            SELECT o.*, ol.* FROM WideWorldImporters.Sales.Orders o
            LEFT JOIN WideWorldImporters.Sales.OrderLines ol
                ON ol.OrderID = o.OrderID
            WHERE o.OrderID = @OrderID
        """;

        // Matching Order and OrderLines has to be done in memory and manually
        // Dapper has no functionality to do this automatically
        var order = connection.Query<Order, OrderLine, Order>(
            sql,
            (order, orderLine) =>
            {
                order.OrderLines.Add(orderLine);
                return order;
            },
            new { OrderID = 530 },
            splitOn: nameof(OrderLine.OrderLineID)
        )
        .GroupBy(o => o.OrderID)
        .Select(g =>
        {
            var groupedOrder = g.First();
            groupedOrder.OrderLines = g.Select(o => o.OrderLines.Single()).ToList();
            return groupedOrder;
        })
        .Single();

        var expectedOrder = new Order
        {
            OrderID = 530,
            CustomerID = 115,
            SalespersonPersonID = 3,
            PickedByPersonID = 13,
            ContactPersonID = 1229,
            BackorderOrderID = null,
            OrderDate = new DateTime(2013, 1, 10),
            ExpectedDeliveryDate = new DateTime(2013, 1, 11),
            CustomerPurchaseOrderNumber = "19446",
            IsUndersupplyBackordered = true,
            Comments = null,
            DeliveryInstructions = null,
            InternalComments = null,
            PickingCompletedWhen = new DateTime(2013, 1, 10, 11, 0, 0),
            LastEditedBy = 13,
            LastEditedWhen = new DateTime(2013, 1, 10, 11, 0, 0),
            OrderLines = null!
        };

        var expectedFirstLine = new OrderLine
        {
            OrderLineID = 1478,
            OrderID = 530,
            StockItemID = 129,
            Description = "Plush shark slippers (Gray) XL",
            PackageTypeID = 7,
            Quantity = 7,
            UnitPrice = 32.00m,
            TaxRate = 15.000m,
            PickedQuantity = 7,
            PickingCompletedWhen = new DateTime(2013, 1, 10, 11, 0, 0),
            LastEditedBy = 13,
            LastEditedWhen = new DateTime(2013, 1, 10, 11, 0, 0)
        };

        Assert.Equal(expectedFirstLine, order.OrderLines.First());

        order.OrderLines = null!; // null to test equality without OrderLines
        Assert.Equal(expectedOrder, order);
    }
}
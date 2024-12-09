using Common;
using PetaPoco;
using PetaPoco.Providers;
using PetaPocoEntities;

namespace PetaPocoFeatures;

[Collection("PetaPoco")]
public class FeatureTests
{
    private readonly IDatabase db = DatabaseConfiguration.Build()
         .UsingConnectionString(DatabaseConfig.MSSQLConnectionString)
         .UsingProvider<SqlServerDatabaseProvider>()
         .UsingDefaultMapper<ConventionMapper>(m =>
         {
             m.InflectTableName = (inflector, s) => inflector.Pluralise(s);
             m.InflectColumnName = (inflector, s) => s;
         })
         .Create();

    [Fact]
    public void A1_EntityIdenticalToTable()
    {
        var order = db.Single<PurchaseOrder>(
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
        var contactInfo = db.Single<SupplierContactInfo>(
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
        string sql = """
            SELECT 
                SupplierId, SupplierName, PhoneNumber, FaxNumber, WebsiteURL, ValidFrom, ValidTo, 
                SupplierId, BankAccountName, BankAccountBranch, BankAccountCode, BankAccountNumber, BankInternationalCode 
            FROM WideWorldImporters.Purchasing.Suppliers 
            WHERE SupplierID = @SupplierID
        """;

        var result = db.Query<SupplierContactInfo, SupplierBankAccount, (SupplierContactInfo, SupplierBankAccount)>(
            (ci, ca) => (ci, ca),
            sql,
            new { SupplierID = 10 }
        );

        var (contactInfo, bankAccount) = result.First();

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
        var orders = db.FetchProc<PurchaseOrderUpdate>(
            "WideWorldImporters.Integration.GetOrderUpdates",
            new { LastCutoff = new DateTime(2014, 1, 1), NewCutoff = new DateTime(2015, 1, 1) }
        );

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

        var sql = Sql.Builder.Where("OrderID = @0", orderId);

        var orderLines = db.Fetch<OrderLine>(
            sql
        );

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

        var actualOrderLine = orderLines.First(ol => ol.OrderLineID == 85261);

        Assert.Equal(expectedFirstOrderLine.OrderLineID, actualOrderLine.OrderLineID);
        Assert.Equal(expectedFirstOrderLine.OrderID, actualOrderLine.OrderID);
        Assert.Equal(expectedFirstOrderLine.StockItemID, actualOrderLine.StockItemID);
        Assert.Equal(expectedFirstOrderLine.Description, actualOrderLine.Description);
        Assert.Equal(expectedFirstOrderLine.PackageTypeID, actualOrderLine.PackageTypeID);
        Assert.Equal(expectedFirstOrderLine.Quantity, actualOrderLine.Quantity);
        Assert.Equal(expectedFirstOrderLine.UnitPrice, actualOrderLine.UnitPrice);
        Assert.Equal(expectedFirstOrderLine.TaxRate, actualOrderLine.TaxRate);
        Assert.Equal(expectedFirstOrderLine.PickedQuantity, actualOrderLine.PickedQuantity);
        Assert.Equal(expectedFirstOrderLine.PickingCompletedWhen, actualOrderLine.PickingCompletedWhen);
        Assert.Equal(expectedFirstOrderLine.LastEditedBy, actualOrderLine.LastEditedBy);
        Assert.Equal(expectedFirstOrderLine.LastEditedWhen, actualOrderLine.LastEditedWhen);
    }

    [Fact]
    public void B2_SelectionOverNonIndexedColumn()
    {
        decimal unitPrice = 25m;

        var orderLines = db.Fetch<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines WHERE UnitPrice = @UnitPrice",
            new { UnitPrice = unitPrice }
        );

        Assert.Equal(13553, orderLines.Count);
        Assert.True(orderLines.All(ol => ol.UnitPrice == unitPrice));
    }

    [Fact]
    public void B3_RangeQuery()
    {
        var from = new DateTime(2014, 12, 20);
        var to = new DateTime(2014, 12, 31);
        var orderLines = db.Fetch<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines WHERE PickingCompletedWhen BETWEEN @Start AND @End",
            new { Start = from, End = to }
        );

        Assert.Equal(1883, orderLines.Count);
        Assert.True(orderLines.All(ol => ol.PickingCompletedWhen >= from && ol.PickingCompletedWhen <= to));
    }

    [Fact]
    public void B4_InQuery()
    {
        var orderIds = new[] { 1, 10, 100, 1000, 10000 };

        var orderLines = db.Fetch<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines WHERE OrderID IN (@0)",
            orderIds
        );

        Assert.Equal(15, orderLines.Count);
        Assert.True(orderLines.All(ol => orderIds.Contains(ol.OrderID)));
    }

    [Fact]
    public void B5_TextSearch()
    {
        string text = "C++";

        var orderLines = db.Fetch<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines WHERE Description LIKE @Text",
            new { Text = $"%{text}%" }
        );

        Assert.Equal(2098, orderLines.Count);
        Assert.True(orderLines.All(ol => ol.Description.Contains(text)));
    }

    [Fact]
    public void B6_PagingQuery()
    {
        int skip = 1000;
        int take = 50;

        var orderLines = db.Fetch<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines ORDER BY OrderLineID OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY",
            new { Skip = skip, Take = take }
        );

        Assert.Equal(50, orderLines.Count);
        Assert.Equal(Enumerable.Range(skip + 1, take), orderLines.Select(ol => ol.OrderLineID));
    }
}

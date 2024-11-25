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
    public void ConnectionTest()
    {
        int count = connection.Query(
            "SELECT * FROM WideWorldImporters.Purchasing.PurchaseOrders"
        ).Count();
        Assert.Equal(2074, count);
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
}


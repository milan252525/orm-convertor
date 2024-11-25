namespace DapperTests;

using System.Data;
using System.Data.SqlClient;
using BenchmarkDotNet.Attributes;
using Common;
using Dapper;
using Dapper.Entities;

[MemoryDiagnoser]
[ExceptionDiagnoser]
public class Benchmarks
{
    private SqlConnection connection = new SqlConnection(DatabaseConfig.MSSQLConnectionString);

    [GlobalSetup]
    public void GlobalSetup()
    {
        /// Stored procedure result has spaces in column names which we can't map onto C# properties
        SqlMapper.SetTypeMap(
            typeof(PurchaseOrderUpdate),
        new CustomPropertyTypeMap(
                typeof(PurchaseOrderUpdate),
                (type, columnName) => type.GetProperty(columnName.Replace(" ", "").Replace("WWI", ""))!
            )
        );

        connection = new SqlConnection(DatabaseConfig.MSSQLConnectionString);
        connection.Open();
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        connection.Dispose();
    }

    [Benchmark]
    public int ConnectionTest()
    {
        return connection.Query(
            "SELECT * FROM WideWorldImporters.Purchasing.PurchaseOrders"
        ).Count();
    }

    [Benchmark]
    public PurchaseOrder A1_EntityIdenticalToTable()
    {
        var order = connection.QuerySingle<PurchaseOrder>(
            "SELECT * FROM WideWorldImporters.Purchasing.PurchaseOrders WHERE PurchaseOrderId = @PurchaseOrderId",
            new { PurchaseOrderId = 25 }
        );

        return order;
    }

    [Benchmark]
    public SupplierContactInfo A2_LimitedEntity()
    {
        var contactInfo = connection.QuerySingle<SupplierContactInfo>(
            "SELECT SupplierID, SupplierName, PhoneNumber, FaxNumber, WebsiteURL, ValidFrom, ValidTo FROM WideWorldImporters.Purchasing.Suppliers WHERE SupplierID = @SupplierID",
            new { SupplierID = 10 }
        );

        return contactInfo;
    }

    [Benchmark]
    public (SupplierContactInfo, SupplierBankAccount) A3_MultipleEntitiesFromOneResult()
    {
        var result = connection.Query<SupplierContactInfo, SupplierBankAccount, (SupplierContactInfo, SupplierBankAccount)>(
            """
                SELECT 
                    SupplierId, SupplierName, PhoneNumber, FaxNumber, WebsiteURL, ValidFrom, ValidTo, 
                    SupplierId, BankAccountName, BankAccountBranch, BankAccountCode, BankAccountNumber, BankInternationalCode 
                FROM WideWorldImporters.Purchasing.Suppliers WHERE SupplierID = @SupplierID
                """,
            (contactInfo, bankAccount) => (contactInfo, bankAccount),
            new { SupplierID = 10 },
            splitOn: nameof(SupplierBankAccount.SupplierID)
        ).Single();

        return result;
    }

    [Benchmark]
    public List<PurchaseOrderUpdate> A4_StoredProcedureToEntity()
    {
        var orders = connection.Query<PurchaseOrderUpdate>(
            "WideWorldImporters.Integration.GetOrderUpdates",
            new { LastCutoff = new DateTime(2014, 1, 1), NewCutoff = new DateTime(2015, 1, 1) },
            commandType: CommandType.StoredProcedure
        ).ToList();

        return orders;
    }
}

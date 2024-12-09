using System.Data;
using System.Data.SqlClient;
using BenchmarkDotNet.Attributes;
using Common;
using Dapper;
using DapperEntities;

namespace DapperPerformance;

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

    [Benchmark]
    public List<OrderLine> B1_SelectionOverIndexedColumn()
    {
        int orderId = 26866;

        var orderLines = connection.Query<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines WHERE OrderID = @OrderID",
            new { OrderID = orderId }
        ).ToList();

        return orderLines;
    }

    [Benchmark]
    public List<OrderLine> B2_SelectionOverNonIndexedColumn()
    {
        decimal unitPrice = 25m;

        var orderLines = connection.Query<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines WHERE UnitPrice = @UnitPrice",
            new { UnitPrice = unitPrice }
        ).ToList();

        return orderLines;
    }

    [Benchmark]
    public List<OrderLine> B3_RangeQuery()
    {
        var from = new DateTime(2014, 12, 20);
        var to = new DateTime(2014, 12, 31);
        var orderLines = connection.Query<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines WHERE PickingCompletedWhen BETWEEN @Start AND @End",
            new { Start = from, End = to }
        ).ToList();

        return orderLines;
    }

    [Benchmark]
    public List<OrderLine> B4_InQuery()
    {
        var orderIds = new[] { 1, 10, 100, 1000, 10000 };

        var orderLines = connection.Query<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines WHERE OrderID IN @OrderIds",
            new { OrderIds = orderIds }
        ).ToList();

        return orderLines;
    }

    [Benchmark]
    public List<OrderLine> B5_TextSearch()
    {
        string text = "C++";

        var orderLines = connection.Query<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines WHERE Description LIKE @Text",
            new { Text = $"%{text}%" }
        ).ToList();

        return orderLines;
    }

    [Benchmark]
    public List<OrderLine> B6_PagingQuery()
    {
        int skip = 1000;
        int take = 50;

        var orderLines = connection.Query<OrderLine>(
            "SELECT * FROM WideWorldImporters.Sales.OrderLines ORDER BY OrderLineID OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY",
            new { Skip = skip, Take = take }
        ).ToList();

        return orderLines;
    }

    [Benchmark]
    public Dictionary<decimal, int> C1_AggregationCount()
    {
        var taxRates = connection.Query<(decimal, int)>(
            """
                SELECT TaxRate, COUNT(TaxRate) as Count 
                FROM WideWorldImporters.Sales.OrderLines 
                GROUP BY TaxRate 
                ORDER BY Count DESC
            """
        ).ToDictionary(x => x.Item1, x => x.Item2);

        return taxRates;
    }

    [Benchmark]
    public decimal C2_AggregationMax()
    {
        var maxUnitPrice = connection.Query<decimal>(
            "SELECT MAX(UnitPrice) FROM WideWorldImporters.Sales.OrderLines"
        ).Single();

        return maxUnitPrice;
    }

    [Benchmark]
    public decimal C3_AggregationSum()
    {
        var totalSales = connection.Query<decimal>(
            "SELECT SUM(Quantity * UnitPrice) FROM WideWorldImporters.Sales.OrderLines"
        ).Single();

        return totalSales;
    }

    public Order D1_OneToManyRelationship()
    {
        string sql = """
            SELECT o.*, ol.* FROM WideWorldImporters.Sales.Orders o
            LEFT JOIN WideWorldImporters.Sales.OrderLines ol
                ON ol.OrderID = o.OrderID
            WHERE o.OrderID = @OrderID
        """;

        var order = connection.Query<Order, OrderLine, Order>(
            sql,
            (order, orderLine) =>
            {
                order.OrderLines.Add(orderLine);
                return order;
            },
            new { OrderID = 530 },
            splitOn: "OrderLineID"
        )
        .GroupBy(o => o.OrderID)
        .Select(g =>
        {
            var groupedOrder = g.First();
            groupedOrder.OrderLines = g.Select(o => o.OrderLines.Single()).ToList();
            return groupedOrder;
        })
        .Single();

        return order;
    }

    [Benchmark]
    public List<StockItem> D2_ManyToManyRelationship()
    {
        string sql = """
            SELECT si.*, sg.*
            FROM WideWorldImporters.Warehouse.StockItems si
            INNER JOIN WideWorldImporters.Warehouse.StockItemStockGroups sisg
                ON si.StockItemID = sisg.StockItemID
            INNER JOIN WideWorldImporters.Warehouse.StockGroups sg
                ON sisg.StockGroupID = sg.StockGroupID
            ORDER BY si.StockItemID
        """;

        var stockItems = new Dictionary<int, StockItem>();

        connection.Query<StockItem, StockGroup, StockItem>(
            sql,
            (stockItem, stockGroup) =>
            {
                if (!stockItems.TryGetValue(stockItem.StockItemID, out var existing))
                {
                    existing = stockItem;
                    existing.StockGroups = [];
                    stockItems.Add(stockItem.StockItemID, existing);
                }

                existing.StockGroups.Add(stockGroup);
                return existing;
            },
            splitOn: nameof(StockGroup.StockGroupID)
        );

        var result = stockItems.Values.ToList();
        return result;
    }

    [Benchmark]
    public List<Customer> D3_OptionalRelationship()
    {
        string sql = """
            SELECT 
        	    c.CustomerID, c.CustomerName, c.AccountOpenedDate, c.CreditLimit, 
        	    ct.CustomerTransactionID, ct.CustomerID, ct.TransactionDate, ct.TransactionAmount
            FROM WideWorldImporters.Sales.Customers c
            LEFT OUTER JOIN WideWorldImporters.Sales.CustomerTransactions ct
        	    ON c.CustomerID = ct.CustomerID
            ORDER BY c.CustomerID
        """;

        var customers = new Dictionary<int, Customer>();

        connection.Query<Customer, CustomerTransaction, Customer>(
            sql,
            (customer, transaction) =>
            {
                if (!customers.TryGetValue(customer.CustomerId, out var existing))
                {
                    existing = customer;
                    customers.Add(customer.CustomerId, existing);
                }

                if (transaction != null)
                {
                    existing.Transactions.Add(transaction);
                }

                return existing;
            },
            splitOn: nameof(CustomerTransaction.CustomerTransactionID)
        );

        var result = customers.Values.ToList();
        return result;
    }

    [Benchmark]
    public List<PurchaseOrder> E1_ColumnSorting()
    {
        var orders = connection.Query<PurchaseOrder>(
            "SELECT TOP (1000) * FROM WideWorldImporters.Purchasing.PurchaseOrders ORDER BY ExpectedDeliveryDate ASC"
        ).ToList();

        return orders;
    }

    [Benchmark]
    public List<string> E2_Distinct()
    {
        var supplierReferences = connection.Query<string>(
            "SELECT DISTINCT SupplierReference FROM WideWorldImporters.Purchasing.PurchaseOrders"
        ).ToList();

        return supplierReferences;
    }

    [Benchmark]
    public List<Person> F1_NestedJSONQuery()
    {
        var sql = """
                SELECT *
                FROM WideWorldImporters.Application.People
                WHERE JSON_VALUE(CustomFields, '$.Title') = @Title
                ORDER BY PersonId
            """;

        var people = connection.Query<Person>(sql, new { Title = "Team Member" }).ToList();

        // triggering JSON parsing
        people.ForEach(p => p.GetCustomFields());

        return people;
    }

    [Benchmark]
    public List<Person> F2_JSONArrayQuery()
    {
        var sql = """
                SELECT *
                FROM WideWorldImporters.Application.People
                WHERE EXISTS (
                    SELECT 1
                    FROM OPENJSON(OtherLanguages)
                    WHERE value = @Language
                )
            """;

        var people = connection.Query<Person>(sql, new { Language = "Slovak" }).ToList();

        // triggering JSON parsing
        people.ForEach(p => p.GetOtherLanguages());

        return people;
    }

    [Benchmark]
    public List<int> G1_Union()
    {
        var suppliers = connection.Query<int>("""
                SELECT SupplierID FROM WideWorldImporters.Purchasing.Suppliers WHERE SupplierID < 5
                UNION
                SELECT SupplierID FROM WideWorldImporters.Purchasing.Suppliers WHERE SupplierID BETWEEN 5 AND 10
                ORDER BY SupplierID
            """
        ).ToList();

        return suppliers;
    }

    [Benchmark]
    public List<int> G2_Intersection()
    {
        var suppliers = connection.Query<int>("""
                SELECT SupplierID FROM WideWorldImporters.Purchasing.Suppliers WHERE SupplierID < 10
                INTERSECT
                SELECT SupplierID FROM WideWorldImporters.Purchasing.Suppliers WHERE SupplierID BETWEEN 5 AND 15
                ORDER BY SupplierID
            """
        ).ToList();

        return suppliers;
    }
}

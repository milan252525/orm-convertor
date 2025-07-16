export const SAMPLES = {
    entity: `namespace Entities;

[Table("Customers", Schema = "Sales")]
public class Customer
{
    [Key]
    public int CustomerID { get; set; }

    public required string CustomerName { get; set; }

    public DateTime AccountOpenedDate { get; set; }

    public decimal? CreditLimit { get; set; }

    public List<CustomerTransaction> Transactions { get; set; } = [];
}

[Table("CustomerTransactions", Schema = "Sales")]
public class CustomerTransaction
{
    [Key]
    public int CustomerTransactionID { get; set; }

    [ForeignKey(nameof(Customer))]
    public int CustomerID { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal TransactionAmount { get; set; }
}

[Table("OrderLines", Schema = "Sales")]
public class OrderLine
{
    [Key]
    public int OrderLineID { get; set; }

    [ForeignKey(nameof(Order))]
    public int OrderID { get; set; }

    public int StockItemID { get; set; }

    public required string Description { get; set; }

    public int PackageTypeID { get; set; }

    public int Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal TaxRate { get; set; }

    public int PickedQuantity { get; set; }

    public DateTime? PickingCompletedWhen { get; set; }

    public int LastEditedBy { get; set; }

    public DateTime LastEditedWhen { get; set; }
}`,
    query1: `public List<OrderLine> Query1()
{
    using var context = contextFactory.CreateDbContext();

    var from = new DateTime(2014, 12, 20);
    var to = new DateTime(2014, 12, 31);

    var orderLines = context.OrderLines
        .Where(ol => ol.PickingCompletedWhen >= from && ol.PickingCompletedWhen <= to)
        .ToList();

    return orderLines;
}`,
    query2: `public List<OrderLine> Query2()
{
    using var context = contextFactory.CreateDbContext();

    string text = "C++";

    var orderLines = context.OrderLines
        .Where(ol => ol.Description.Contains(text))
        .ToList();

    return orderLines;
}`,
    query3: `public List<Customer> Query3()
{
    using var context = contextFactory.CreateDbContext();

    var result = context.Customers
        .Include(c => c.Transactions)
        .OrderBy(c => c.CustomerID)
        .ToList();

    return result;
}`,
    entityTarget: `namespace Entities;

public class Customer
{
    public int CustomerID { get; set; }

    public required string CustomerName { get; set; }

    public DateTime AccountOpenedDate { get; set; }

    public decimal? CreditLimit { get; set; }

    public List<CustomerTransaction> Transactions { get; set; } = [];
}

public class CustomerTransaction
{
    public int CustomerTransactionID { get; set; }

    public int CustomerID { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal TransactionAmount { get; set; }
}

public class OrderLine
{
    public int OrderLineID { get; set; }

    public int OrderID { get; set; }
    
    public int StockItemID { get; set; }
    
    public required string Description { get; set; }
    
    public int PackageTypeID { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal? UnitPrice { get; set; }
    
    public decimal TaxRate { get; set; }
    
    public int PickedQuantity { get; set; }
    
    public DateTime? PickingCompletedWhen { get; set; }
    
    public int LastEditedBy { get; set; }
    
    public DateTime LastEditedWhen { get; set; }
}`,
    tquery1: `public List<OrderLine> Query1()
{
    var from = new DateTime(2014, 12, 20);
    var to = new DateTime(2014, 12, 31);

    var orderLines = connection.Query<OrderLine>(
        "SELECT * FROM WideWorldImporters.Sales.OrderLines WHERE PickingCompletedWhen BETWEEN @Start AND @End",
        new { Start = from, End = to }
    ).ToList();

    return orderLines;
}`,
    tquery2: `public List<OrderLine> Query2()
{
    string text = "C++";

    var orderLines = connection.Query<OrderLine>(
        "SELECT * FROM WideWorldImporters.Sales.OrderLines WHERE Description LIKE @Text",
        new { Text = $"%{text}%" }
    ).ToList();

    return orderLines;
}`,
    tquery3: `public List<Customer> Query3()
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
            if (!customers.TryGetValue(customer.CustomerID, out var existing))
            {
                existing = customer;
                customers.Add(customer.CustomerID, existing);
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
    return result;`
};
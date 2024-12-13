using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreEntities;

[Table("Customers", Schema = "Sales")]
public class Customer
{
    public int CustomerId { get; set; }

    public required string CustomerName { get; set; }

    public DateTime AccountOpenedDate { get; set; }

    public decimal CreditLimit { get; set; }

    public List<CustomerTransaction> Transactions { get; set; } = [];
}

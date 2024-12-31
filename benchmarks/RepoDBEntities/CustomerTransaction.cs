using System.ComponentModel.DataAnnotations.Schema;

namespace RepoDBEntities;

[Table("CustomerTransactions", Schema = "Sales")]
public class CustomerTransaction
{
    public int CustomerTransactionID { get; set; }

    public int CustomerID { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal TransactionAmount { get; set; }
}

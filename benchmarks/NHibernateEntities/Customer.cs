namespace NHibernateEntities;

public class Customer
{
    public virtual int CustomerId { get; set; }

    public virtual required string CustomerName { get; set; }

    public virtual DateTime AccountOpenedDate { get; set; }

    public virtual decimal? CreditLimit { get; set; }

    public virtual IList<CustomerTransaction> Transactions { get; set; } = [];
}

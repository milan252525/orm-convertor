namespace DapperTests.Entities;
public class PurchaseOrder
{
    public int PurchaseOrderID { get; set; }

    public int SupplierID { get; set; }

    public DateTime OrderDate { get; set; }
}

using Microsoft.EntityFrameworkCore;

namespace EFCoreEntities;

public class WWIContext(DbContextOptions<WWIContext> options) : DbContext(options)
{
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
}

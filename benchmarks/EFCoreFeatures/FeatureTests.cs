using Common;
using EFCoreEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFCoreFeatures;

[Collection("EFCore")]
public class FeatureTests
{
    private readonly PooledDbContextFactory<WWIContext> contextFactory;

    public FeatureTests()
    {
        var options = new DbContextOptionsBuilder<WWIContext>()
            .UseSqlServer(DatabaseConfig.MSSQLConnectionString)
            .Options;

        contextFactory = new(options);
    }

    [Fact]
    public void A1_EntityIdenticalToTable()
    {
        using var context = contextFactory.CreateDbContext();

        var order = context.PurchaseOrders.Find(25);

        Assert.NotNull(order);
        Assert.Multiple(() =>
        {
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
        });
    }
}

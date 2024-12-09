using PetaPoco;
using PetaPoco.Providers;
using Common;

namespace PetaPocoFeatures;
[Collection("PetaPoco")]
public class FeatureTests
{
    private readonly IDatabase db = DatabaseConfiguration.Build()
         .UsingConnectionString(DatabaseConfig.MSSQLConnectionString)
         .UsingProvider<SqlServerDatabaseProvider>()
         .UsingDefaultMapper<ConventionMapper>(m =>
         {
             m.InflectTableName = (inflector, s) => inflector.Pluralise(inflector.Underscore(s));
         })
         .Create();

    [Fact]
    public void A1_EntityIdenticalToTable()
    {
        /// var order = connection.QuerySingle<PurchaseOrder>(
        //"SELECT * FROM WideWorldImporters.Purchasing.PurchaseOrders WHERE PurchaseOrderId = @PurchaseOrderId",
        //    new { PurchaseOrderId = 25 }
        //);

        var order = db.Single
    }
}

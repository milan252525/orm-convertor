using Model.AbstractRepresentation;
using Model.AbstractRepresentation.Enums;

namespace Tests.SampleData;

public static class CustomerMapEFCore
{
    public const string Source = """
        namespace EFCoreEntities;

        using System.ComponentModel.DataAnnotations;
        using System.ComponentModel.DataAnnotations.Schema;

        [Table("Customers", Schema = "Sales")]
        public class Customer
        {
            [Key]
            public required int CustomerID { get; set; }

            [MaxLength(200)]
            public required string CustomerName { get; set; }

            [Precision(7)]
            public required DateTime AccountOpenedDate { get; set; }

            [Precision(18, 2)]
            public decimal? CreditLimit { get; set; }

            public List<CustomerTransaction> Transactions { get; set; } = [];

        }

        """;

    public static EntityMap Map
    {
        get
        {
            var map = new EntityMap
            {
                Entity = new Entity
                {
                    Name = "Customer",
                    Namespace = "EFCoreEntities",
                    AccessModifier = AccessModifier.Public,
                    Attributes = []
                },
                Table = "Customers",
                Schema = "Sales",
                PropertyMaps = [
                    new() {
                       Property = new Property
                       {
                           Name = "CustomerID",
                           Type = "int",
                           AccessModifier = AccessModifier.Public,
                           OtherModifiers = ["required"],
                           HasGetter = true,
                           HasSetter = true,
                       },
                       IsNullable = false,
                       OtherDatabaseProperties = new Dictionary<string, string>
                       {
                           { "IsPrimaryKey", "true" },
                           { "PrimaryKeyStrategy", ((int)PrimaryKeyStrategy.Identity).ToString() },
                       }
                   },
                   new() {
                       Property = new Property
                       {
                           Name = "CustomerName",
                           Type = "string",
                           AccessModifier = AccessModifier.Public,
                           OtherModifiers = ["required"],
                           HasGetter = true,
                           HasSetter = true
                       },
                       Length = 200,
                       IsNullable = false,
                   },
                   new() {
                       Property = new Property
                       {
                           Name = "AccountOpenedDate",
                           Type = "DateTime",
                           AccessModifier = AccessModifier.Public,
                           OtherModifiers = ["required"],
                           HasGetter = true,
                           HasSetter = true
                       },
                       Precision = 7,
                       IsNullable = false,
                   },
                   new() {
                       Property = new Property
                       {
                           Name = "CreditLimit",
                           Type = "decimal",
                           IsNullable = true,
                           AccessModifier = AccessModifier.Public,
                           HasGetter = true,
                           HasSetter = true
                       },
                       Precision = 18,
                       Scale = 2,
                       IsNullable = true,
                   },
                   new() {
                       Property = new Property
                       {
                           Name = "Transactions",
                           Type = "List<CustomerTransaction>",
                           AccessModifier = AccessModifier.Public,
                           HasGetter = true,
                           HasSetter = true,
                           DefaultValue = "[]",
                       },
                       Relation = new() {
                           Cardinality = Cardinality.OneToMany,
                           Source = "Customer",
                           Target = "CustomerTransaction",
                       },
                       IsNullable = false,
                       OtherDatabaseProperties = new Dictionary<string, string>
                       {
                           { "IsForeignKey", "true" },
                           { "ForeignKeyCardinality", ((int)Cardinality.OneToMany).ToString() },
                       }
                   },
               ],
            };

            foreach (var propertyMap in map.PropertyMaps)
            {
                map.Entity.Properties.Add(propertyMap.Property);
            }

            return map;
        }
    }
}

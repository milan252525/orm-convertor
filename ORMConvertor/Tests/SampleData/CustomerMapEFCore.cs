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
                           HasGetter = true,
                           HasSetter = true,
                       },
                       OtherDatabaseProperties = new Dictionary<string, string>
                       {
                           { "IsPrimaryKey", "true" },
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
                       IsNullable = false,
                       Type = "string",
                       Length = 200
                   },
                   new() {
                       Property = new Property
                       {
                           Name = "AccountOpenedDate",
                           Type = "DateTime",
                           AccessModifier = AccessModifier.Public,
                           HasGetter = true,
                           HasSetter = true
                       },
                       IsNullable = false,
                       Type = "datetime2",
                       Precision = 7
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
                       IsNullable = true,
                       Type = "decimal",
                       Precision = 18,
                       Scale = 2
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
                       OtherDatabaseProperties = new Dictionary<string, string>
                       {
                           { "IsForeignKey", "true" },
                           { "ForeignKeyCardinality", ((int)Cardinality.OneToMany).ToString() }
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

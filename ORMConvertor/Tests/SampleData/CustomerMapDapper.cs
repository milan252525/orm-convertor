using Model.AbstractRepresentation.Enums;
using Model.AbstractRepresentation;

namespace Tests.SampleData;

public class CustomerMapDapper
{
    public const string Source = """
        namespace DapperEntities;

        public class Customer
        {
            public int CustomerID { get; set; }

            public required string CustomerName { get; set; }

            public DateTime AccountOpenedDate { get; set; }

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
                    Namespace = "DapperEntities",
                    AccessModifier = AccessModifier.Public,
                    Attributes = [],
                },
                Table = default,
                Schema = default,
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
                       }
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
                       //Relations = [
                       //    new() {
                       //        Cardinality = Cardinality.OneToMany,
                       //        Source = "Customer",
                       //        Target = "CustomerTransaction",
                       //    },
                       //]
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

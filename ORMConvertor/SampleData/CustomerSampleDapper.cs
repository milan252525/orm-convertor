using Model.AbstractRepresentation;
using Model.AbstractRepresentation.Enums;

namespace SampleData;

public class CustomerSampleDapper
{
    public const string Entity = """
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
                           Type = new() { CLRType = CLRType.Int },
                           AccessModifier = AccessModifier.Public,
                           HasGetter = true,
                           HasSetter = true,
                       },
                   },
                   new() {
                       Property = new Property
                       {
                           Name = "CustomerName",
                           Type = new() { CLRType = CLRType.String },
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
                           Type = new() { CLRType = CLRType.DateTime },
                           AccessModifier = AccessModifier.Public,
                           HasGetter = true,
                           HasSetter = true
                       },
                   },
                   new() {
                       Property = new Property
                       {
                           Name = "CreditLimit",
                           Type = new() { CLRType = CLRType.Decimal },
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
                           Type = new() { CLRType = CLRType.List, GenericParam = "CustomerTransaction"},
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

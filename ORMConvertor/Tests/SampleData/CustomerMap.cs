using Model.AbstractRepresentation;
using Model.AbstractRepresentation.Enums;

namespace Tests.SampleData;

public static class CustomerMap
{
    public const string Source = """
        using System.ComponentModel.DataAnnotations;
        using System.ComponentModel.DataAnnotations.Schema;

        namespace EFCoreEntities;

        [Table("Customers", Schema = "Sales")]
        public class Customer
        {
            [Key]
            public int CustomerID { get; set; }

            public required string CustomerName { get; set; }

            public DateTime AccountOpenedDate { get; set; }

            public decimal? CreditLimit { get; set; }

            public List<CustomerTransaction> Transactions { get; set; } = [];
        }
        """;

    public static EntityMap Map => new()
    {
        Entity = new Entity
        {
            Name = "Person",
            Namespace = "EFCoreEntities",
            AccessModifier = AccessModifier.Public,
            Attributes = [
                new() {
                    Name = "Table",
                    PositionalParameters = ["Customers"],
                    NamedParameters = new() {
                        ["Schema"] = "Sales",
                    },
                },
            ],
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
                    Attributes = [
                        new() {
                            Name = "Key",
                        },
                    ],
                },
            },
            new() {
                Property = new Property
                {
                    Name = "CustomerName",
                    Type = "int",
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
                },
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
                Relations = [
                    new() {
                        Cardinality = Cardinality.OneToMany,
                        Source = "Customer",
                        Target = "CustomerTransaction",
                    },
                ]
            },
        ],
    };
}

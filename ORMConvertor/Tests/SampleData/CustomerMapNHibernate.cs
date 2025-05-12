using Model.AbstractRepresentation;
using Model.AbstractRepresentation.Enums;

namespace Tests.SampleData;
public class CustomerMapNHibernate
{
    public const string SourceEntity = """
        namespace NHibernateEntities;

        public class Customer
        {
            public virtual int CustomerID { get; set; }

            public virtual required string CustomerName { get; set; }

            public virtual DateTime AccountOpenedDate { get; set; }

            public virtual decimal? CreditLimit { get; set; }

            public virtual List<CustomerTransaction> Transactions { get; set; } = [];
        }
        """;

    public const string SourceMapping = """
        <?xml version="1.0" encoding="utf-8" ?>
        <hibernate-mapping xmlns="urn:nhibernate-mapping-2.2" namespace="NHibernateEntities">
            <class name="NHibernateEntities.Customer, NHibernateEntities" table="Customers" schema="Sales">
                <id name="CustomerID" column="CustomerID" type="int">
                    <generator class="identity" />
                </id>
                <property name="CustomerName" not-null="true" />
                <property name="AccountOpenedDate" not-null="true" />
                <property name="CreditLimit" not-null="false" />
                <bag name="Transactions" inverse="true" cascade="all-delete-orphan">
                    <key column="CustomerID" />
                    <one-to-many class="CustomerTransaction" />
                </bag>
            </class>
        </hibernate-mapping>
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
                    Namespace = "NHibernateEntities",
                    AccessModifier = AccessModifier.Public,
                    Attributes = [],
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
                           OtherModifiers = ["virtual"],
                           HasGetter = true,
                           HasSetter = true,
                       },
                       OtherDatabaseProperties = new Dictionary<string, string>
                       {
                           { "IsPrimaryKey", "true" },
                           { "PrimaryKeyStrategy", ((int)PrimaryKeyStrategy.Identity).ToString() }
                       }
                   },
                   new() {
                       Property = new Property
                       {
                           Name = "CustomerName",
                           Type = "string",
                           AccessModifier = AccessModifier.Public,
                           OtherModifiers = ["virtual", "required"],
                           HasGetter = true,
                           HasSetter = true
                       },
                       IsNullable = false,
                   },
                   new() {
                       Property = new Property
                       {
                           Name = "AccountOpenedDate",
                           Type = "DateTime",
                           AccessModifier = AccessModifier.Public,
                           OtherModifiers = ["virtual"],
                           HasGetter = true,
                           HasSetter = true
                       },
                       IsNullable = false,
                   },
                   new() {
                       Property = new Property
                       {
                           Name = "CreditLimit",
                           Type = "decimal",
                           IsNullable = true,
                           AccessModifier = AccessModifier.Public,
                           OtherModifiers = ["virtual"],
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
                           OtherModifiers = ["virtual"],
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
                       ],
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

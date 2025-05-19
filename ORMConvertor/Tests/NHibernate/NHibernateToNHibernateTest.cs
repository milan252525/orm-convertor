using AbstractWrappers;
using NHibernateWrappers;
using Tests.SampleData;

namespace Tests.NHibernate;

public class NHibernateToNHibernateTest
{
    [Fact]
    public void NHibernateToNHibernateOverall()
    {
        var sourceEntity = """
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

        var sourceMapping = """
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

        AbstractEntityBuilder builder = new NHibernateEntityBuilder();
        var entityParser = new NHibernateEntityParser(builder);
        var mappingParser = new NHibernateXMLMappingParser(builder);
        entityParser.Parse(sourceEntity);
        mappingParser.Parse(sourceMapping);

        var results = builder.Build();
        var entityOutput = results.Single(x => x.ContentType == Model.ContentType.CSharp);
        var xmlOutput = results.Single(x => x.ContentType == Model.ContentType.XML);

        Assert.Multiple(() =>
        {
            Assert.Equal(Model.ContentType.CSharp, entityOutput.ContentType);
            Assert.Equal(sourceEntity, entityOutput.Content.Trim());

            Assert.Equal(Model.ContentType.XML, xmlOutput.ContentType);
            Assert.Equal(sourceMapping, xmlOutput.Content.Trim());
        });
    }
}

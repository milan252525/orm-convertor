using AbstractWrappers;
using NHibernateWrappers;
using Tests.SampleData;

namespace Tests.NHibernate;

public class AbstractToNHibernateTest
{
    [Fact]
    public void AbstractToNHibernateOverall()
    {
        var source = CustomerMapNHibernate.Map;

        AbstractEntityBuilder builder = new NHibernateEntityBuilder
        {
            EntityMap = source
        };

        var results = builder.Build();
        var entityOutput = results.Single(x => x.ContentType == Model.ContentType.CSharp);
        var xmlOutput = results.Single(x => x.ContentType == Model.ContentType.XML);

        var expectedEntity = """
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

        var expectedMapping = """
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

        Assert.Multiple(() =>
        {
            Assert.Equal(Model.ContentType.CSharp, entityOutput.ContentType);
            Assert.Equal(expectedEntity, entityOutput.Content.Trim());

            Assert.Equal(Model.ContentType.XML, xmlOutput.ContentType);
            Assert.Equal(expectedMapping, xmlOutput.Content.Trim());
        });
    }
}

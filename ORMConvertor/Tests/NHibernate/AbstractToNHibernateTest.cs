using AbstractWrappers;
using Model;
using NHibernateWrappers;
using SampleData;

namespace Tests.NHibernate;

public class AbstractToNHibernateTest
{
    [Fact]
    public void AbstractToNHibernateOverall()
    {
        var source = CustomerSampleNHibernate.Map;

        AbstractEntityBuilder builder = new NHibernateEntityBuilder
        {
            EntityMap = source
        };

        var results = builder.Build();
        var entityOutput = results.Single(x => x.ContentType == ContentType.CSharpEntity);
        var xmlOutput = results.Single(x => x.ContentType == ContentType.XML);

        Assert.Multiple(() =>
        {
            Assert.Equal(ContentType.CSharpEntity, entityOutput.ContentType);
            Assert.Equal(CustomerSampleNHibernate.Entity, entityOutput.Content, ignoreLineEndingDifferences: true);

            Assert.Equal(ContentType.XML, xmlOutput.ContentType);
            Assert.Equal(CustomerSampleNHibernate.XmlMapping, xmlOutput.Content, ignoreLineEndingDifferences: true);
        });
    }
}

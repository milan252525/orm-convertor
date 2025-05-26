using AbstractWrappers;
using Model;
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
        var entityOutput = results.Single(x => x.ContentType == ContentType.CSharp);
        var xmlOutput = results.Single(x => x.ContentType == ContentType.XML);

        Assert.Multiple(() =>
        {
            Assert.Equal(ContentType.CSharp, entityOutput.ContentType);
            Assert.Equal(CustomerMapNHibernate.SourceEntity, entityOutput.Content, ignoreLineEndingDifferences: true);

            Assert.Equal(ContentType.XML, xmlOutput.ContentType);
            Assert.Equal(CustomerMapNHibernate.SourceMapping, xmlOutput.Content, ignoreLineEndingDifferences: true);
        });
    }
}

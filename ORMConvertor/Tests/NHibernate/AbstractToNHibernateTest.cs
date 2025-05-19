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

        Assert.Multiple(() =>
        {
            Assert.Equal(Model.ContentType.CSharp, entityOutput.ContentType);
            Assert.Equal(CustomerMapNHibernate.SourceEntity, entityOutput.Content);

            Assert.Equal(Model.ContentType.XML, xmlOutput.ContentType);
            Assert.Equal(CustomerMapNHibernate.SourceMapping, xmlOutput.Content);
        });
    }
}

using AbstractWrappers;
using Model;
using NHibernateWrappers;
using Tests.SampleData;

namespace Tests.NHibernate;

public class NHibernateToNHibernateTest
{
    [Fact]
    public void NHibernateToNHibernateOverall()
    {
        AbstractEntityBuilder builder = new NHibernateEntityBuilder();
        var entityParser = new NHibernateEntityParser(builder);
        var mappingParser = new NHibernateXMLMappingParser(builder);
        entityParser.Parse(CustomerMapNHibernate.SourceEntity);
        mappingParser.Parse(CustomerMapNHibernate.SourceMapping);

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

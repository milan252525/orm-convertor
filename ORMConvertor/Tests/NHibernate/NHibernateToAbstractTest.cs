using AbstractWrappers;
using Newtonsoft.Json;
using NHibernateWrappers;
using SampleData;

namespace Tests.NHibernate;
public class NHibernateToAbstractTest
{
    [Fact]
    public void NHibernateToAbstractOverall()
    {
        // Using dummy builder because we need a concrete implementation,
        // however we will only be inspecting how the abstract representation is built
        AbstractEntityBuilder builder = new DummyEntityBuilder();  
        var entityParser = new NHibernateEntityParser(builder);
        var mappingParser = new NHibernateXMLMappingParser(builder);
        entityParser.Parse(CustomerSampleNHibernate.Entity);
        mappingParser.Parse(CustomerSampleNHibernate.XmlMapping);

        Assert.Equal(JsonConvert.SerializeObject(CustomerSampleNHibernate.Map), JsonConvert.SerializeObject(builder.EntityMap), ignoreLineEndingDifferences: true);
    }
}

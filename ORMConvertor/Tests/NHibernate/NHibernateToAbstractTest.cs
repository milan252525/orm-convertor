using AbstractWrappers;
using DapperWrappers;
using Newtonsoft.Json;
using NHibernateWrappers;
using Tests.SampleData;

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
        entityParser.Parse(CustomerMapNHibernate.SourceEntity);
        mappingParser.Parse(CustomerMapNHibernate.SourceMapping);

        Assert.Equal(JsonConvert.SerializeObject(CustomerMapNHibernate.Map), JsonConvert.SerializeObject(builder.EntityMap));
    }
}

using SampleData;

namespace ORMConvertorAPI.Data;

public static class Samples
{
    public static Dictionary<int, string> GetSamples => new()
    {
        { 1, CustomerSampleDapper.Entity },
        { 2, CustomerSampleNHibernate.Entity },
        { 3, CustomerSampleNHibernate.XmlMapping },
        { 4, CustomerSampleEFCore.Entity},
        { 5, CustomerSampleEFCore.Query },
    };
}

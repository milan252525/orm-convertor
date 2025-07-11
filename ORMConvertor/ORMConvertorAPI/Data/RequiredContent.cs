using Model;
using Model.APIModels;

namespace ORMConvertorAPI.Data;
public static class RequiredContent
{
    public static List<RequiredContentDefinition> GetRequiredContent => [
        new (ORMEnum.Dapper, [
            new(1, ConversionContentType.CSharpEntity, "Entity (C# class)")
        ]),
        new (ORMEnum.NHibernate, [
            new (2, ConversionContentType.CSharpEntity, "Entity (C# class)"),
            new (3, ConversionContentType.XML, "Mapping File (XML)"),
        ]),
        new (ORMEnum.EFCore, [
            new(4, ConversionContentType.CSharpEntity, "Entity (C# class)"),
            new (5, ConversionContentType.CSharpQuery, "LINQ Query (optional, wrapped in a method)"),
        ]),
    ];
}

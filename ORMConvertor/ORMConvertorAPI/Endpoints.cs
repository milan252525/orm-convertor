using AbstractWrappers;
using DapperWrappers;
using EFCoreWrappers;
using Model;
using NHibernateWrappers;

namespace ORMConvertorAPI;

public static class Endpoints
{
    public record RequiredContentUnit(int Id, ContentType ContentType, string Description);
    public record RequiredContentDefinition(ORMType OrmType, List<RequiredContentUnit> Required);

    public static List<RequiredContentDefinition> requiredContent = [
        new (ORMType.Dapper, [new(1, ContentType.CSharp, "Entity (C# class)")]),
        new (ORMType.NHibernate, [
            new (2, ContentType.CSharp, "Entity (C# class)"),
            new (3, ContentType.XML, "Mapping file (XML)"),
        ]),
        new (ORMType.EFCore, [new(4, ContentType.CSharp, "Entity (C# class)")]),
    ];
    public record ConvertRequest(ORMType SourceOrm, ORMType TargetOrm, List<ConversionSource> Sources);
    public record ConvertResponse(List<ConversionSource> Sources);


    public static void Map(WebApplication app)
    {
        app.MapGet("/required-content", () => requiredContent);

        app.MapPost("/convert",
            (ConvertRequest req) =>
            {
                var builder = GetBuilder(req.TargetOrm);

                if (builder == null)
                {
                    return Results.BadRequest("Target ORM not supported");
                }

                var parsers = GetParsers(req.SourceOrm, builder);

                if (parsers.Count == 0)
                {
                    return Results.BadRequest("Source ORM not supported");
                }

                var results = new List<ConversionSource>();

                foreach (var parser in parsers)
                {
                    var parsable = req.Sources
                        .Where(x => parser.CanParse(x.ContentType))
                        .FirstOrDefault();

                    if (parsable != null)
                    {
                        parser.Parse(parsable.Content);
                    }
                }

                results.AddRange(builder.Build());

                return Results.Ok(new ConvertResponse(results));
            })
           .WithName("Convert")
           .Produces<ConvertResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .WithOpenApi();
    }

    private static AbstractEntityBuilder? GetBuilder(ORMType ormType)
    {
        return ormType switch
        {
            ORMType.Dapper => new DapperEntityBuilder(),
            ORMType.NHibernate => new NHibernateEntityBuilder(),
            ORMType.EFCore => new EFCoreEntityBuilder(),
            _ => null
        };
    }

    private static List<IParser> GetParsers(ORMType ormType, AbstractEntityBuilder entityBuilder)
    {
        return ormType switch
        {
            ORMType.Dapper => [
                new DapperEntityParser(entityBuilder)
            ],
            ORMType.NHibernate => [
                new NHibernateEntityParser(entityBuilder),
                new NHibernateXMLMappingParser(entityBuilder)
            ],
            _ => []
        };
    }
}

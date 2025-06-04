using AbstractWrappers;
using DapperWrappers;
using EFCoreWrappers;
using Model;
using NHibernateWrappers;
using SampleData;

namespace ORMConvertorAPI;

public static class Endpoints
{
    private record RequiredContentUnit(int Id, ContentType ContentType, string Description);
    private record RequiredContentDefinition(ORMType OrmType, List<RequiredContentUnit> Required);

    private static List<RequiredContentDefinition> requiredContent = [
        new (ORMType.Dapper, [
            new(1, ContentType.CSharpEntity, "Entity (C# class)")
        ]),
        new (ORMType.NHibernate, [
            new (2, ContentType.CSharpEntity, "Entity (C# class)"),
            new (3, ContentType.XML, "Mapping File (XML)"),
        ]),
        new (ORMType.EFCore, [
            new(4, ContentType.CSharpEntity, "Entity (C# class)"),
            new (5, ContentType.CSharpQuery, "LINQ Query (optional, wrapped in a method)"),
        ]),
    ];
    private record ConvertRequest(ORMType SourceOrm, ORMType TargetOrm, List<ConversionSource> Sources);
    private record ConvertResponse(List<ConversionSource> Sources);

    private static readonly Dictionary<int, string> samples = new()
    {
        { 1, CustomerSampleDapper.Entity },
        { 2, CustomerSampleNHibernate.Entity },
        { 3, CustomerSampleNHibernate.XmlMapping },
        { 4, CustomerSampleEFCore.Entity},
        { 5, CustomerSampleEFCore.Query },
    };

    public static void Map(WebApplication app)
    {
        app.MapGet("/required-content", () => requiredContent)
            .WithName("RequiredContent")
            .Produces<List<RequiredContentDefinition>>(StatusCodes.Status200OK)
            .WithOpenApi();

        app.MapPost("/convert", ConvertHandler)
           .WithName("Convert")
           .Produces<ConvertResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .WithOpenApi();

        app.MapGet("samples", () => samples)
            .Produces<Dictionary<int, string>>(StatusCodes.Status200OK)
            .WithOpenApi();
    }

    private static IResult ConvertHandler(ConvertRequest req)
    {
        var entityBuilder = GetEntityBuilder(req.TargetOrm);
        var queryBuilder = GetQueryBuilder(req.TargetOrm);

        if (entityBuilder == null)
            return Results.BadRequest("Target ORM not supported");

        var parsers = GetParsers(req.SourceOrm, entityBuilder, queryBuilder);
        if (parsers.Count == 0)
            return Results.BadRequest("Source ORM not supported");

        var results = new List<ConversionSource>();

        foreach (var parser in parsers)
        {
            if (parser.CanParse(ContentType.CSharpQuery) && queryBuilder == null)
                continue;

            var parsable = req.Sources.FirstOrDefault(x => parser.CanParse(x.ContentType));
            if (parsable == null) continue;

            if (parser is IQueryParser qp)
                qp.Parse(parsable.Content, entityBuilder.EntityMap);
            else
                parser.Parse(parsable.Content);
        }

        results.AddRange(entityBuilder.Build());
        if (queryBuilder != null) results.AddRange(queryBuilder.Build());

        return Results.Ok(new ConvertResponse(results));
    }

    private static AbstractEntityBuilder? GetEntityBuilder(ORMType ormType)
    {
        return ormType switch
        {
            ORMType.Dapper => new DapperEntityBuilder(),
            ORMType.NHibernate => new NHibernateEntityBuilder(),
            ORMType.EFCore => new EFCoreEntityBuilder(),
            _ => null
        };
    }
    private static AbstractQueryBuilder? GetQueryBuilder(ORMType ormType)
    {
        return ormType switch
        {
            ORMType.Dapper => new DapperSqlQueryBuilder(),
            ORMType.NHibernate => null,
            ORMType.EFCore => null,
            _ => null
        };
    }


    private static List<IParser> GetParsers(ORMType ormType, AbstractEntityBuilder entityBuilder, AbstractQueryBuilder? queryBuilder)
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
            ORMType.EFCore => [
                new EFCoreEntityParser(entityBuilder),
                new EFCoreLinqQueryParser(queryBuilder!)
            ],
            _ => []
        };
    }
}

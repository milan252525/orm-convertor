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
        new (ORMType.Dapper, [new(1, ContentType.CSharpEntity, "Entity (C# class)")]),
        new (ORMType.NHibernate, [
            new (2, ContentType.CSharpEntity, "Entity (C# class)"),
            new (3, ContentType.XML, "Mapping File (XML)"),
        ]),
        new (ORMType.EFCore, [
            new(4, ContentType.CSharpEntity, "Entity (C# class)"),
            new (5, ContentType.CSharpQuery, "LINQ Query (optional, wrapped in a method)"),
        ]),
    ];
    public record ConvertRequest(ORMType SourceOrm, ORMType TargetOrm, List<ConversionSource> Sources);
    public record ConvertResponse(List<ConversionSource> Sources);


    public static void Map(WebApplication app)
    {
        app.MapGet("/required-content", () => requiredContent);

        app.MapPost("/convert",
            (ConvertRequest req) =>
            {
                var entityBuilder = GetEntityBuilder(req.TargetOrm);
                var queryBuilder = GetQueryBuilder(req.TargetOrm);

                if (entityBuilder == null)
                {
                    return Results.BadRequest("Target ORM not supported");
                }

                var parsers = GetParsers(req.SourceOrm, entityBuilder, queryBuilder);

                if (parsers.Count == 0)
                {
                    return Results.BadRequest("Source ORM not supported");
                }

                var results = new List<ConversionSource>();

                foreach (var parser in parsers)
                {
                    // Skip parsing queries when target query builder is not yet implemented
                    if (parser.CanParse(ContentType.CSharpQuery) && queryBuilder == null)
                    {
                        continue;
                    }

                    var parsable = req.Sources
                        .Where(x => parser.CanParse(x.ContentType))
                        .FirstOrDefault();

                    if (parsable == null)
                    {
                        continue;
                    }

                    if (parser is IQueryParser queryParser)
                    {
                        queryParser.Parse(parsable.Content, entityBuilder.EntityMap);
                    }
                    else
                    {
                        parser.Parse(parsable.Content);
                    }
                }

                results.AddRange(entityBuilder.Build());
                if (queryBuilder != null)
                {
                    results.AddRange(queryBuilder.Build());
                }

                return Results.Ok(new ConvertResponse(results));
            })
           .WithName("Convert")
           .Produces<ConvertResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .WithOpenApi();
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

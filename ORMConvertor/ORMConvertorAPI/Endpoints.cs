using AbstractWrappers;
using DapperWrappers;
using Model;
using NHibernateWrappers;

namespace ORMConvertorAPI;

public static class Endpoints
{

    public sealed record ConvertRequest(ORMType SourceOrm, ORMType TargetOrm, List<ConversionSource> Sources);

    public sealed record ConvertResponse(List<ConversionSource> Sources);

    public static void Map(WebApplication app)
    {
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
                    var content = req.Sources
                        .Where(x => parser.CanParse(x.ContentType))
                        .First().Content;

                    parser.Parse(content);
                }

                results.AddRange(builder.Build());

                return Results.Ok(new ConvertResponse(req.Sources));
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

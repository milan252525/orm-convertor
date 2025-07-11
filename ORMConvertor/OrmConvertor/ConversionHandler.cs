using AbstractWrappers;
using Model;
using OrmConvertor.Factories;

namespace OrmConvertor;

public static class ConversionHandler
{
    public static List<ConversionSource> Convert(
        ORMEnum sourceOrm,
        ORMEnum targetOrm,
        List<ConversionSource> sources
    )
    {
        var entityBuilder = EntityBuilderFactory.Create(targetOrm);
        var queryBuilder = QueryBuilderFactory.Create(targetOrm);

        if (entityBuilder == null)
        {
            throw new InvalidOperationException("Target ORM not supported");
        }

        var parsers = ParserFactory.Create(sourceOrm, entityBuilder, queryBuilder);

        if (parsers.Count == 0)
        {
            throw new InvalidOperationException("Source ORM not supported");
        }

        var results = new List<ConversionSource>();

        foreach (var parser in parsers)
        {
            if (parser.CanParse(ConversionContentType.CSharpQuery) && queryBuilder == null)
            {
                continue;
            }

            var parsable = sources.FirstOrDefault(x => parser.CanParse(x.ContentType));
            if (parsable == null)
            {
                continue;
            }

            if (parser is IQueryParser qp)
            {
                qp.Parse(parsable.Content, entityBuilder.EntityMap);
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

        return results;
    }
}

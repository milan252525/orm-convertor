using AbstractWrappers;
using DapperWrappers;
using EFCoreWrappers;
using Model;
using NHibernateWrappers;

namespace OrmConvertor.Factories;
internal class ParserFactory
{
    static Dictionary<ORMEnum, Func<AbstractEntityBuilder, AbstractQueryBuilder?, IEnumerable<IParser>>> Map =>
        new()
        {
            [ORMEnum.Dapper] = (eb, qb) => [
                new DapperEntityParser(eb)
            ],
            [ORMEnum.NHibernate] = (eb, qb) => [
                new NHibernateEntityParser(eb),
                new NHibernateXMLMappingParser(eb)
            ],
            [ORMEnum.EFCore] = (eb, qb) => [
                new EFCoreEntityParser(eb),
                new EFCoreLinqQueryParser(qb!)
            ]
        };

    public static List<IParser> Create(ORMEnum orm, AbstractEntityBuilder eb, AbstractQueryBuilder? qb) =>
        Map.TryGetValue(orm, out var factory)
            ? factory(eb, qb).ToList()
            : [];
}

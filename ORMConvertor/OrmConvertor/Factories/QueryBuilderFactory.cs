using AbstractWrappers;
using DapperWrappers;
using Model;

namespace OrmConvertor.Factories;
internal static class QueryBuilderFactory
{
    static Dictionary<ORMEnum, Func<AbstractQueryBuilder?>> Map =>
        new()
        {
            [ORMEnum.Dapper] = () => new DapperSqlQueryBuilder(),
            [ORMEnum.NHibernate] = () => null,
            [ORMEnum.EFCore] = () => null
        };

    public static AbstractQueryBuilder? Create(ORMEnum orm) =>
        Map.TryGetValue(orm, out var ctor) ? ctor() : null;
}
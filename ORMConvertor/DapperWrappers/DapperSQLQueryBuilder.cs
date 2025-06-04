using AbstractWrappers;
using Model.Exceptions;
using Model.QueryInstructions;
using System.Text;

namespace DapperWrappers;

public class DapperSqlQueryBuilder : AbstractQueryBuilder
{
    private readonly IQueryVisitor visitor = new DapperSQLQueryVisitor();

    public override string Build()
    {
        return BuildSelectQuery();
    }

    private string BuildSelectQuery()
    {
        // Own builder for nesting
        StringBuilder sqlBuilder = new();

        BuildProjectionPart(sqlBuilder);

        BuildFromPart(sqlBuilder);

        BuildJoinPart(sqlBuilder);

        BuildWherePart(sqlBuilder);

        BuildGroupByPart(sqlBuilder);

        BuildHavingPart(sqlBuilder);

        BuildOrderByPart(sqlBuilder);

        return sqlBuilder.ToString();
    }
    private void BuildProjectionPart(StringBuilder sqlBuilder)
    {
        sqlBuilder.Append("SELECT ");

        var projections = instructions.OfType<ProjectInstruction>().ToList();

        if (projections.Count == 0)
        {
            sqlBuilder.Append('*');
            return;
        }

        foreach (var projection in projections)
        {
            sqlBuilder.Append(projection.Accept(visitor) + ", ");
        }

        sqlBuilder.Remove(sqlBuilder.Length - 2, 2); // Remove trailing comma and space
        sqlBuilder.AppendLine();
    }

    private void BuildFromPart(StringBuilder sqlBuilder)
    {
        var fromInstructions = instructions.OfType<FromInstruction>().ToList();

        if (fromInstructions.Count != 1)
        {
            throw new QueryBuilderException("None or too many query sources.");
        }

        sqlBuilder.Append("FROM ");
        sqlBuilder.Append(fromInstructions.First().Accept(visitor));
        sqlBuilder.AppendLine();
    }

    private void BuildJoinPart(StringBuilder sqlBuilder)
    {
        var joinInstructions = instructions.OfType<JoinInstruction>().ToList();

        foreach (var join in joinInstructions)
        {
            sqlBuilder.AppendLine(join.Accept(visitor));
        }
    }

    private void BuildWherePart(StringBuilder sqlBuilder)
    {
        var selectInstructions = instructions.OfType<SelectInstruction>().ToList();
        
        if (selectInstructions.Count == 0)
        {
            return;
        }

        sqlBuilder.Append("WHERE ");
        for (int i = 0; i < selectInstructions.Count; i++)
        {
            sqlBuilder.Append(selectInstructions[i].Accept(visitor));

            if (i < selectInstructions.Count - 1)
            {
                sqlBuilder.AppendLine();
                sqlBuilder.Append("\tAND ");
            }
        }
        sqlBuilder.AppendLine();
    }

    private void BuildOrderByPart(StringBuilder sqlBuilder)
    {
        var orderByInstructions = instructions.OfType<OrderByInstruction>().ToList();
        
        if (orderByInstructions.Count == 0)
        {
            return;
        }

        sqlBuilder.Append("ORDER BY ");
        foreach (var orderBy in orderByInstructions)
        {
            sqlBuilder.Append(orderBy.Accept(visitor) + ", ");
        }
        sqlBuilder.Remove(sqlBuilder.Length - 2, 2); // Remove trailing comma and space
        sqlBuilder.AppendLine();
    }

    private void BuildGroupByPart(StringBuilder sqlBuilder)
    {
        var groupByInstructions = instructions.OfType<GroupByInstruction>().ToList();
        
        if (groupByInstructions.Count == 0)
        {
            return;
        }

        sqlBuilder.Append("GROUP BY ");
        foreach (var groupBy in groupByInstructions)
        {
            sqlBuilder.Append(groupBy.Accept(visitor) + ", ");
        }
        sqlBuilder.Remove(sqlBuilder.Length - 2, 2); // Remove trailing comma and space
        sqlBuilder.AppendLine();
    }

    private void BuildHavingPart(StringBuilder sqlBuilder)
    {
        var havingInstructions = instructions.OfType<HavingInstruction>().ToList();
        if (havingInstructions.Count == 0)
        {
            return;
        }

        sqlBuilder.Append("HAVING ");
        for (int i = 0; i < havingInstructions.Count; i++)
        {
            sqlBuilder.Append(havingInstructions[i].Accept(visitor));

            if (i < havingInstructions.Count - 1)
            {
                sqlBuilder.AppendLine();
                sqlBuilder.Append("\tAND ");
            }
        }
        sqlBuilder.AppendLine();
    }
}
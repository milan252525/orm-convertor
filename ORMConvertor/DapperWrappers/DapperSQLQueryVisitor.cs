using Model.Exceptions;
using Model.QueryInstructions;
using Model.QueryInstructions.Enums;

namespace DapperWrappers;

public class DapperSQLQueryVisitor : IQueryVisitor
{
    public string Visit(FromInstruction instr)
    {
        var alias = instr.Alias is null ? string.Empty : $" AS {instr.Alias}";
        return $"{instr.Table}{alias}";
    }

    public string Visit(ProjectInstruction instr)
    {
        string value;
        if (instr.Function != null)
        {
            value = $"{instr.Function}({instr.Table}.{instr.Attribute})";
        }
        else
        {
            value = $"{instr.Table}.{instr.Attribute}";
        }

        var alias = instr.Alias is null ? string.Empty : $" AS {instr.Alias}";
        return $"{value}{alias}";
    }

    public string Visit(SelectInstruction instr)
    {
        string left;
        if (instr.LeftTable != null && instr.LeftProperty != null)
        {
            left = $"{instr.LeftTable}.{instr.LeftProperty}";
        }
        else if (instr.LeftConstant != null)
        {
            left = instr.LeftConstant.Replace('"', '\'');
        }
        else
        {
            throw new QueryBuilderException("SelectInstruction: Left side must be a table.column or a constant.");
        }

        string right;
        if (instr.RightTable != null && instr.RightProperty != null)
        {
            right = $"{instr.RightTable}.{instr.RightProperty}";
        }
        else if (instr.RightConstant != null)
        {
            right = instr.RightConstant.Replace('"', '\'');
        }
        else
        {
            throw new QueryBuilderException("SelectInstruction: Right side must be a table.column or a constant.");
        }


        string op = instr.Operator switch
        {
            BooleanOperator.Equal => "=",
            BooleanOperator.NotEqual => "<>",
            BooleanOperator.GreaterThan => ">",
            BooleanOperator.GreaterThanOrEqual => ">=",
            BooleanOperator.LessThan => "<",
            BooleanOperator.LessThanOrEqual => "<=",
            BooleanOperator.Like => "LIKE",
            BooleanOperator.NotLike => "NOT LIKE",
            BooleanOperator.In => "IN",
            BooleanOperator.NotIn => "NOT IN",
            _ => throw new QueryBuilderException($"Unsupported BooleanOperator: {instr.Operator}")
        };

        return $"{left} {op} {right}";
    }

    public string Visit(JoinInstruction instr)
    {
        string joinType = instr.Kind switch
        {
            JoinKind.Inner => "INNER JOIN",
            JoinKind.Left => "LEFT JOIN",
            JoinKind.Right => "RIGHT JOIN",
            JoinKind.Full => "FULL JOIN",
            _ => "JOIN"
        };

        var rightTable = instr.RightTableAlias is null
            ? instr.RightTable
            : $"{instr.RightTable} {instr.RightTableAlias}";

        return $"{joinType} {rightTable} ON {instr.LeftTable}.{instr.LeftProperty} = {(instr.RightTableAlias ?? instr.RightTable)}.{instr.RightProperty}";
    }

    public string Visit(OrderByInstruction instr)
    {
        string column = instr.Table != null
            ? $"{instr.Table}.{instr.Attribute}"
            : instr.Attribute;
        string direction = instr.Asc ? "ASC" : "DESC";
        return $"{column} {direction}";
    }

    public string Visit(GroupByInstruction instr)
    {
        return $"{instr.Table}.{instr.Attribute}";
    }

    public string Visit(HavingInstruction instr)
    {
        string left = BuildOperand(instr.LeftTable, instr.LeftProperty, instr.LeftConstant, instr.LeftFunction);
        string right = BuildOperand(instr.RightTable, instr.RightProperty, instr.RightConstant, instr.RightFunction);

        string op = instr.Operator switch
        {
            BooleanOperator.Equal => "=",
            BooleanOperator.NotEqual => "<>",
            BooleanOperator.GreaterThan => ">",
            BooleanOperator.GreaterThanOrEqual => ">=",
            BooleanOperator.LessThan => "<",
            BooleanOperator.LessThanOrEqual => "<=",
            BooleanOperator.Like => "LIKE",
            BooleanOperator.NotLike => "NOT LIKE",
            BooleanOperator.In => "IN",
            BooleanOperator.NotIn => "NOT IN",
            _ => throw new QueryBuilderException($"Unsupported BooleanOperator: {instr.Operator}")
        };

        return $"{left} {op} {right}";
    }

    private static string BuildOperand(
        string? table,
        string? property,
        string? constant,
        string? function
    )
    {
        if (property != null)
        {
            string column = table != null ? $"{table}.{property}" : property;
            return function != null
                ? $"{function}({column})"
                : column;
        }
        else if (constant != null)
        {
            string value = constant.Replace('"', '\'');
            return function != null
                ? $"{function}({value})"
                : value;
        }
        else
        {
            throw new QueryBuilderException($"Having operand must be a table.column or a constant.");
        }
    }

}

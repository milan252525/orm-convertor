using Model;
using Model.QueryInstructions;
using Model.QueryInstructions.Enums;

namespace AbstractWrappers;

public abstract class AbstractQueryBuilder()
{
    protected readonly List<QueryInstruction> instructions = [];
    protected readonly Stack<int> marks = [];
    private SetOperationInstruction? initiatedSetOperation = null;

    public void Push()
    {
        marks.Push(instructions.Count);
    }

    public void Pop()
    {
        var start = marks.Pop();
        var body = instructions.GetRange(start, instructions.Count - start);
        instructions.RemoveRange(start, instructions.Count - start);

        // Store instruction into a subquery, unless there is an ongoing set operation.
        if (initiatedSetOperation != null)
        {
            var newSetOp = new SetOperationInstruction(
                initiatedSetOperation.OperationType,
                initiatedSetOperation.Left,
                new SubQueryInstruction(body)
            );
            instructions.Add(newSetOp);
            initiatedSetOperation = null;
        }
        else
        {
            instructions.Add(new SubQueryInstruction(body));
        }
    }

    public void From(string table, string? alias = null)
    {
        instructions.Add(new FromInstruction(table, alias));
    }

    public void Project(string table, string attr, string? alias = null, string? function = null)
    {
        instructions.Add(new ProjectInstruction(table, attr, alias, function));
    }

    public void Select(
        string? leftTable, string? leftProperty, string? leftConstant,
        BooleanOperator op,
        string? rightTable, string? rightProperty, string? rightConstant)
    {
        instructions.Add(new SelectInstruction(leftTable, leftProperty, leftConstant, op, rightTable, rightProperty, rightConstant));
    }

    public void Join(JoinKind kind, string left, string right, string leftProperty, string rightProperty, string? rightTableAlias = null)
    {
        instructions.Add(new JoinInstruction(kind, left, right, rightTableAlias, leftProperty, rightProperty));
    }

    public void GroupBy(string table, string attr)
    {
        instructions.Add(new GroupByInstruction(table, attr));
    }

    public void OrderBy(string? table, string attributeOrAlias, bool asc = true)
    {
        instructions.Add(new OrderByInstruction(table, attributeOrAlias, asc));
    }

    public void Having(
        string? leftTable, string? leftProperty, string? leftConstant, string? leftFunction,
        BooleanOperator op,
        string? rightTable, string? rightProperty, string? rightConstant, string? rightFunction
    )
    {
        instructions.Add(new HavingInstruction(leftTable, leftProperty, leftConstant, leftFunction, op, rightTable, rightProperty, rightConstant, rightFunction));
    }

    public void SetOperation(SetOperationType operation)
    {
        if (instructions.Count == 0 || instructions[0] is not SubQueryInstruction subQuery)
        {
            throw new InvalidOperationException("Set operation can only be initiated after a subquery has been defined. Use Push() to start a subquery and Pop() to end it.");
        }
        instructions.RemoveAt(0);

        initiatedSetOperation = new SetOperationInstruction(operation, subQuery, new SubQueryInstruction([]));
    }

    public abstract List<ConversionSource> Build();
}

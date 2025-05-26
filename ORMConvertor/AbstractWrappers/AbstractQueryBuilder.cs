using Model.QueryInstructions;
using Model.QueryInstructions.Enums;

namespace AbstractWrappers;

public abstract class AbstractQueryBuilder()
{
    protected readonly List<QueryInstruction> instructions = [];
    protected readonly Stack<int> marks = [];

    public void Push()
    {
        marks.Push(instructions.Count);
    }

    public void Pop()
    {
        var start = marks.Pop();
        var body = instructions.GetRange(start, instructions.Count - start);
        instructions.RemoveRange(start, instructions.Count - start);
        instructions.Add(new SubQueryInstruction(body));
    }

    public void From(string table)
    {
        instructions.Add(new FromInstruction(table));
    }

    public void Project(string table, string attr, string? alias = null)
    {
        instructions.Add(new ProjectInstruction(table, attr, alias));
    }

    public void Select(string expr)
    {
        instructions.Add(new SelectInstruction(expr));
    }

    public void Join(string left, string right, JoinKind kind, string on)
    {
        instructions.Add(new JoinInstruction(left, right, kind, on));
    }

    public void Aggregate(string table, string[] attrs, string fn)
    {
        instructions.Add(new AggregateInstruction(table, attrs, fn));
    }

    public void OrderBy(string table, string[] attrs, bool desc = false)
    {
        instructions.Add(new OrderByInstruction(table, attrs, desc));
    }

    public abstract string Build();
}

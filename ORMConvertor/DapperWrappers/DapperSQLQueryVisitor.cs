using Model.QueryInstructions;
using System.Text;

namespace DapperWrappers;

public class DapperSQLQueryVisitor : IQueryVisitor
{
    private readonly StringBuilder stringBuilder = new();

    public string Finish()
    {
        return stringBuilder.ToString();
    }

    public void Visit(FromInstruction instr)
    {
        stringBuilder.AppendLine($"FROM {instr.Table}");
    }

    public void Visit(ProjectInstruction instr)
    {
        var alias = instr.Alias is null ? string.Empty : $" AS {instr.Alias}";
        stringBuilder.AppendLine($"SELECT {instr.Table}.{instr.Attribute}{alias}");
    }

    public void Visit(SelectInstruction instr)
    {
        stringBuilder.AppendLine($"WHERE {instr.Expression}");
    }

    public void Visit(JoinInstruction instr)
    {
        stringBuilder.AppendLine($"{instr.LeftTable} {instr.Kind.ToString().ToUpper()} JOIN {instr.RightTable} ON {instr.Condition}");
    }

    public void Visit(AggregateInstruction instr)
    {
        stringBuilder.AppendLine($"GROUP BY {string.Join(",", instr.Attributes.Select(a => $"{instr.Table}.{a}"))}");
    }

    public void Visit(OrderByInstruction instr)
    {
        stringBuilder.AppendLine($"ORDER BY {string.Join(",", instr.Attributes.Select(a => $"{instr.Table}.{a}"))} {(instr.Desc ? "DESC" : "ASC")}");
    }
}

using Model.QueryInstructions;
using System.Text;

namespace EFCoreWrappers;

public class EFCoreLinqQueryVisitor : IQueryVisitor
{
    private readonly StringBuilder stringBuilder = new();

    public string Finish()
    {
        return stringBuilder.ToString();
    }

    public void Visit(FromInstruction instr)
    {
        throw new NotImplementedException();
    }

    public void Visit(ProjectInstruction instr)
    {
        throw new NotImplementedException();
    }

    public void Visit(SelectInstruction instr)
    {
        throw new NotImplementedException();
    }

    public void Visit(JoinInstruction instr)
    {
        throw new NotImplementedException();
    }

    public void Visit(AggregateInstruction instr)
    {
        throw new NotImplementedException();
    }

    public void Visit(OrderByInstruction instr)
    {
        throw new NotImplementedException();
    }
}

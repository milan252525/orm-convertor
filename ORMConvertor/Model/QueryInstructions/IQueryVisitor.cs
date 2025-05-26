using Model;

namespace Model.QueryInstructions;

public interface IQueryVisitor
{
    public void Visit(FromInstruction instr);
    public void Visit(ProjectInstruction instr);
    public void Visit(SelectInstruction instr);
    public void Visit(JoinInstruction instr);
    public void Visit(AggregateInstruction instr);
    public void Visit(OrderByInstruction instr);
    public string Finish();
}

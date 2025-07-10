using Model;

namespace Model.QueryInstructions;

public interface IQueryVisitor
{
    public string Visit(FromInstruction instr);
    public string Visit(ProjectInstruction instr);
    public string Visit(SelectInstruction instr);
    public string Visit(JoinInstruction instr);
    public string Visit(GroupByInstruction instr);
    public string Visit(OrderByInstruction instr);
    public string Visit(HavingInstruction instr);
    public string Visit(SetOperationInstruction instr);
}

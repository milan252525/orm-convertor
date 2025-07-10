using Model.QueryInstructions.Enums;

namespace Model.QueryInstructions;
public sealed record SetOperationInstruction(
    SetOperationType OperationType,
    SubQueryInstruction Left,
    SubQueryInstruction Right
) : QueryInstruction
{
    public override string Accept(IQueryVisitor visitor) => visitor.Visit(this);
}

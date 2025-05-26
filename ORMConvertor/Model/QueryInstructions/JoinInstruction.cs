using Model.QueryInstructions.Enums;

namespace Model.QueryInstructions;

public sealed record JoinInstruction(
    string LeftTable,
    string RightTable,
    JoinKind Kind,
    string Condition
) : QueryInstruction
{
    public override void Accept(IQueryVisitor visitor) => visitor.Visit(this);
}

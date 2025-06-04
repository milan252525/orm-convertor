using Model.QueryInstructions.Enums;

namespace Model.QueryInstructions;

public sealed record JoinInstruction(
    JoinKind Kind,
    string LeftTable,
    string RightTable,
    string? RightTableAlias,
    string LeftProperty,
    string RightProperty
) : QueryInstruction
{
    public override string Accept(IQueryVisitor visitor) => visitor.Visit(this);
}

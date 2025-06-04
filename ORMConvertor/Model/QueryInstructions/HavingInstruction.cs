using Model.QueryInstructions.Enums;

namespace Model.QueryInstructions;

public sealed record HavingInstruction(
    string? LeftTable,
    string? LeftProperty,
    string? LeftConstant,
    string? LeftFunction,
    BooleanOperator Operator,
    string? RightTable,
    string? RightProperty,
    string? RightConstant,
    string? RightFunction
) : QueryInstruction
{
    public override string Accept(IQueryVisitor visitor) => visitor.Visit(this);
}

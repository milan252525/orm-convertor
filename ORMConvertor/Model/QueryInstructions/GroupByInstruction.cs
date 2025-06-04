namespace Model.QueryInstructions;

public sealed record GroupByInstruction(
    string Table,
    string Attribute
) : QueryInstruction
{
    public override string Accept(IQueryVisitor visitor) => visitor.Visit(this);
}

namespace Model.QueryInstructions;

public sealed record OrderByInstruction(
    string? Table,
    string Attribute,
    bool Asc = true
) : QueryInstruction
{
    public override string Accept(IQueryVisitor visitor) => visitor.Visit(this);
}

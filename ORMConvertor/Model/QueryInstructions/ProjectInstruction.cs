namespace Model.QueryInstructions;

public sealed record ProjectInstruction(
    string Table,
    string Attribute,
    string? Alias = null,
    string? Function = null
) : QueryInstruction
{
    public override string Accept(IQueryVisitor visitor) => visitor.Visit(this);
}

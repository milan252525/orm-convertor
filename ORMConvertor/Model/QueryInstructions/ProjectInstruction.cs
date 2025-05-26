namespace Model.QueryInstructions;

public sealed record ProjectInstruction(
    string Table,
    string Attribute,
    string? Alias
) : QueryInstruction
{
    public override void Accept(IQueryVisitor visitor) => visitor.Visit(this);
}

namespace Model.QueryInstructions;

public sealed record OrderByInstruction(
    string Table,
    string[] Attributes,
    bool Desc
) : QueryInstruction
{
    public override void Accept(IQueryVisitor visitor) => visitor.Visit(this);
}

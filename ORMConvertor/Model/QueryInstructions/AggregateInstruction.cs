namespace Model.QueryInstructions;

public sealed record AggregateInstruction(
    string Table,
    string[] Attributes,
    string Function
) : QueryInstruction
{
    public override void Accept(IQueryVisitor visitor) => visitor.Visit(this);
}

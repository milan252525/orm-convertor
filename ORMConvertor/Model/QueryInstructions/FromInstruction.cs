namespace Model.QueryInstructions;

public sealed record FromInstruction(string Table) : QueryInstruction
{
    public override void Accept(IQueryVisitor visitor) => visitor.Visit(this);
}

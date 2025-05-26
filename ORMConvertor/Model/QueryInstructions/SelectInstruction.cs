namespace Model.QueryInstructions;
public sealed record SelectInstruction(string Expression) : QueryInstruction
{
    public override void Accept(IQueryVisitor visitor) => visitor.Visit(this);
}

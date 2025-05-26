namespace Model.QueryInstructions;

public abstract record QueryInstruction
{
    public abstract void Accept(IQueryVisitor visitor);
}

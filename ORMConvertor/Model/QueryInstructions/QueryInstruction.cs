namespace Model.QueryInstructions;

public abstract record QueryInstruction
{
    public abstract string Accept(IQueryVisitor visitor);
}

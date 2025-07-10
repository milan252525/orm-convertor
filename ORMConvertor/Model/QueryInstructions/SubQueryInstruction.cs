namespace Model.QueryInstructions;

public sealed record SubQueryInstruction(List<QueryInstruction> Instructions) : QueryInstruction
{
    public override string Accept(IQueryVisitor v)
    {
        foreach (var instr in Instructions)
        {
            instr.Accept(v);
        }

        // TODO
        return string.Empty;
    }
}

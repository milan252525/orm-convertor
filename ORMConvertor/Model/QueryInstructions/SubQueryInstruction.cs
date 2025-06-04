namespace Model.QueryInstructions;

public sealed record SubQueryInstruction(IReadOnlyList<QueryInstruction> Body) : QueryInstruction
{
    public override string Accept(IQueryVisitor v)
    {
        foreach (var instr in Body)
        {
            instr.Accept(v);
        }

        // TODO
        return string.Empty;
    }
}

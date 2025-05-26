using AbstractWrappers;

namespace EFCoreWrappers;

public class EFCoreLinqQueryBuilder : AbstractQueryBuilder
{
    public override string Build()
    {
        var visitor = new EFCoreLinqQueryVisitor();
        foreach (var instr in instructions)
        {
            instr.Accept(visitor);
        }

        return visitor.Finish();
    }
}

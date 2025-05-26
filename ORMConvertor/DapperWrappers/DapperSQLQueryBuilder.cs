using AbstractWrappers;

namespace DapperWrappers;

public class DapperSQLQueryBuilder : AbstractQueryBuilder
{
    public override string Build()
    {
        var visitor = new DapperSQLQueryVisitor();
        foreach (var instr in instructions)
        {
            instr.Accept(visitor);
        }

        return visitor.Finish();
    }
}

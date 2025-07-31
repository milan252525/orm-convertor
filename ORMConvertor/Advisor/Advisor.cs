using System.Runtime.InteropServices;

namespace Advisor;

public unsafe partial class Advisor
{
    [LibraryImport("libadvisor.so", EntryPoint = "ilp_solve")]
    private static partial int IlpSolve(
        long* mem,
        double* cost,
        int* z,
        long MEM, int N, int Q, int F,
        int* out_objective,
        int* out_selected,
        int* out_assignment
    );

    /// <param name="mem">Allocated memory (Q*F)</param>
    /// <param name="cost">Runtime cost (Q*F)</param>
    /// <param name="z">Query weights (Q)</param>
    /// <param name="MEM">Max memory constraint</param>
    /// <param name="N">Max number of selected frameworks</param>
    /// <param name="Q">Number of queries</param>
    /// <param name="F">Number of frameworks</param>
    /// <param name="objective">Output: Best combined runtime</param>
    /// <param name="selected">Output: Selected frameworks for best runtime (out array, length F)</param>
    /// <param name="assignment">Output: Final framework assignment per query (out array, length Q)</param>
    /// <returns>0 on success, negative for error</returns>
    public static int Solve(
        long[] mem,
        double[] cost,
        int[] z,
        long MEM, int N, int Q, int F,
        out int objective,
        int[] selected,
        int[] assignment
        )
    {
        if (mem.Length < Q * F)
        {
            throw new ArgumentException("Memory array too short");
        }

        if (cost.Length < Q * F)
        {
            throw new ArgumentException("Cost array too short");
        }

        if (z.Length < Q)
        {
            throw new ArgumentException("Z array (query weights) too short");
        }

        if (selected.Length < F)
        {
            throw new ArgumentException("Selected array too short");
        }

        if (assignment.Length < Q)
        {
            throw new ArgumentException("Assignment array too short");
        }

        int obj = 0;
        int ret;
        fixed (long* pmem = mem)
        fixed (double* pcost = cost)
        fixed (int* pz = z)
        fixed (int* psel = selected)
        fixed (int* passign = assignment)
        {
            ret = IlpSolve(pmem, pcost, pz, MEM, N, Q, F, &obj, psel, passign);
        }
        objective = obj;
        return ret;
    }
}

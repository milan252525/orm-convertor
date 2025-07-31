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

    /// <param name="mem">long[] memory requirements (Q*F)</param>
    /// <param name="cost">double[] cost table (Q*F)</param>
    /// <param name="z">int[] query weights (Q)</param>
    /// <param name="MEM">total memory</param>
    /// <param name="N">max number of frameworks</param>
    /// <param name="Q">number of queries</param>
    /// <param name="F">number of frameworks</param>
    /// <param name="objective">out int for optimal value</param>
    /// <param name="selected">output: framework selected (out array, length F)</param>
    /// <param name="assignment">output: per-query assignment (out array, length Q)</param>
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
            throw new ArgumentException("mem array too short");
        }

        if (cost.Length < Q * F)
        {
            throw new ArgumentException("cost array too short");
        }

        if (z.Length < Q)
        {
            throw new ArgumentException("z array too short");
        }

        if (selected.Length < F)
        {
            throw new ArgumentException("selected array too short");
        }

        if (assignment.Length < Q)
        {
            throw new ArgumentException("assignment array too short");
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

namespace ORMConvertorAPI.Dtos.Advisor;

public record AdvisorSolveRequest(
    long[] Memory, // flat array Q*F
    double[] Cost, // flat array Q*F
    int[] Z, // length Q
    long MEM,
    int N,
    int Q,
    int F
);

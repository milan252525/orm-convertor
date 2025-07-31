namespace ORMConvertorAPI.Dtos.Advisor;

public record AdvisorSolveResponse(
    int Status,
    int Objective,
    int[] Selected,
    int[] Assignment
);

namespace Model.QueryInstructions.Enums;

public enum BooleanOperator
{
    Equal = 1,              // =
    NotEqual = 2,           // <>
    GreaterThan = 3,        // >
    GreaterThanOrEqual = 4, // >=
    LessThan = 5,           // <
    LessThanOrEqual = 6,    // <=
    Like = 7,               // LIKE
    NotLike = 8,            // NOT LIKE
    In = 9,                 // IN (...)
    NotIn = 10,             // NOT IN (...)
}

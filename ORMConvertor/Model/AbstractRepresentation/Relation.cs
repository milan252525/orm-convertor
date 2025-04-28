using Model.AbstractRepresentation.Enums;

namespace Model.AbstractRepresentation;

public class Relation
{
    public required string Source { get; set; }
    public required string Target { get; set; }
    public required Cardinality Cardinality { get; set; }
}

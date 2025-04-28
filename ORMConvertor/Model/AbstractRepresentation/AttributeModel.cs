namespace Model.AbstractRepresentation;

public class AttributeModel
{
    public required string Name { get; set; }

    public List<string> PositionalParameters { get; set; } = [];

    public Dictionary<string, string> NamedParameters { get; set; } = [];
}

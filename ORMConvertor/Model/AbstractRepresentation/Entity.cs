using Model.AbstractRepresentation.Enums;

namespace Model.AbstractRepresentation;

public class Entity
{
    public required string Name { get; set; }

    public AccessModifier? AccessModifier { get; set; }

    public List<AttributeModel> Attributes { get; set; } = [];

    public List<Property> Properties { get; set; } = [];

    public string? Namespace { get; set; }
}

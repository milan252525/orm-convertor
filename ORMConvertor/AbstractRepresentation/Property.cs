using ORMConvertor.Generators;

namespace ORMConvertor.AbstractRepresentation;

public record class Property : ICodeElement
{
    public string? Name { get; set; }

    public DataType? DataType { get; set; }

    public List<string> Modifiers { get; set; } = [];

    public bool HasGetter { get; set; } = false;

    public bool HasSetter { get; set; } = false;

    public T Accept<T>(ICodeGenerationVisitor<T> visitor)
    {
        return visitor.VisitProperty(this);
    }
}

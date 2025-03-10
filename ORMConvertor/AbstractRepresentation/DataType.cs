using ORMConvertor.Generators;

namespace ORMConvertor.AbstractRepresentation;
public record class DataType : ICodeElement
{
    public Type? CSharpType { get; set; }

    public bool IsNullable { get; set; } = false;

    public T Accept<T>(ICodeGenerationVisitor<T> visitor)
    {
        return visitor.VisitDataType(this);
    }
}

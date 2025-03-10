using ORMConvertor.AbstractRepresentation;

namespace ORMConvertor.Generators;

public interface ICodeGenerationVisitor<T>
{
    T VisitProperty(Property property);
    T VisitDataType(DataType dataType);
}

using ORMConvertor.AbstractRepresentation;

namespace ORMConvertor.Parsers;
public interface IPropertyParser
{
    Property Parse(string propertyCode);
}

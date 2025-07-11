using Model.AbstractRepresentation;

namespace AbstractWrappers;
public interface IQueryParser : IParser
{
    void Parse(string source, EntityMap? entityMap = null);
}
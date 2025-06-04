using Model;
using Model.AbstractRepresentation;

namespace AbstractWrappers;

public interface IParser
{
    void Parse(string source);

    bool CanParse(ContentType contentType);
}

public interface IQueryParser : IParser
{
    void Parse(string source, EntityMap? entityMap = null);
}
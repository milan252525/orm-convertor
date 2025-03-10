using ORMConvertor.AbstractRepresentation;

namespace ORMConvertor.Generators;
public class PropertyGenerator : ICodeGenerationVisitor<string>
{
    private readonly Dictionary<Type, string> _typeMapping = new Dictionary<Type, string>
    {
        { typeof(int), "int" },
        { typeof(string), "string" },
        { typeof(bool), "bool" },
        { typeof(double), "double" },
        { typeof(float), "float" },
        { typeof(decimal), "decimal" },
        { typeof(long), "long" },
        { typeof(short), "short" },
        { typeof(byte), "byte" },
        { typeof(char), "char" }
    };

    public string VisitDataType(DataType dataType)
    {
        if (dataType.CSharpType != null && _typeMapping.TryGetValue(dataType.CSharpType, out var alias))
        {
            return alias + (dataType.IsNullable ? "?" : "");
        }

        return dataType.CSharpType?.Name + (dataType.IsNullable ? "?" : "") ?? "object";
    }

    public string VisitProperty(Property property)
    {
        var modifiersStr = property.Modifiers != null && property.Modifiers.Any()
            ? $"{string.Join(" ", property.Modifiers)} "
            : "";

        var typeString = property.DataType != null ? property.DataType.Accept(this) : "object";

        var accessors = new List<string>();
        if (property.HasGetter)
            accessors.Add("get;");
        if (property.HasSetter)
            accessors.Add("set;");
        var accessorsStr = string.Join(" ", accessors);

        return $"{modifiersStr}{typeString} {property.Name} {{ {accessorsStr} }}";
    }
}

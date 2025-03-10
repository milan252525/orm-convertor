using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ORMConvertor.AbstractRepresentation;

namespace ORMConvertor.Parsers;
public class PropertyParser : IPropertyParser
{
    public Property Parse(string propertyCode)
    {
        var tree = CSharpSyntaxTree.ParseText(propertyCode);
        var root = tree.GetRoot();

        var propertyDeclaration = root.DescendantNodes()
                                      .OfType<PropertyDeclarationSyntax>()
                                      .FirstOrDefault();

        if (propertyDeclaration == null)
        {
            throw new Exception("No property declaration found in the provided code.");
        }

        var name = propertyDeclaration.Identifier.Text;

        var typeSyntax = propertyDeclaration.Type;
        var typeText = typeSyntax.ToString();
        bool isNullable = typeSyntax is NullableTypeSyntax;

        var modifiers = propertyDeclaration.Modifiers
                                           .Select(m => m.ValueText)
                                           .ToList();

        var hasGetter = propertyDeclaration.AccessorList?.Accessors
                        .Any(a => a.Keyword.ValueText == "get") ?? false;
        var hasSetter = propertyDeclaration.AccessorList?.Accessors
                        .Any(a => a.Keyword.ValueText == "set") ?? false;

        var dataType = new DataType
        {
            CSharpType = ResolveType(typeText),
            IsNullable = isNullable
        };

        return new Property
        {
            Name = name,
            DataType = dataType,
            HasGetter = hasGetter,
            HasSetter = hasSetter,
            Modifiers = modifiers
        };
    }

    private Type ResolveType(string typeName)
    {
        var trimmed = typeName.TrimEnd('?');

        var typeMapping = new Dictionary<string, Type>
        {
            { "int", typeof(int) },
            { "string", typeof(string) },
            { "bool", typeof(bool) },
            { "double", typeof(double) },
            { "float", typeof(float) },
            { "decimal", typeof(decimal) },
            { "long", typeof(long) },
            { "short", typeof(short) },
            { "byte", typeof(byte) },
            { "char", typeof(char) }
        };

        if (typeMapping.TryGetValue(trimmed, out var resolvedType))
        {
            return resolvedType;
        }

        return Type.GetType(trimmed) ?? typeof(object);
    }
}

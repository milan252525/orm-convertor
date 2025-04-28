using Model.AbstractRepresentation.Enums;

namespace AbstractWrappers.Convertors;
public static class AccessModifierConvertor
{
    public static AccessModifier? FromString(string? modifier)
    {
        return modifier switch
        {
            "public" => AccessModifier.Public,
            "private" => AccessModifier.Private,
            "protected" => AccessModifier.Protected,
            "internal" => AccessModifier.Internal,
            "protected internal" => AccessModifier.ProtectedInternal,
            "private protected" => AccessModifier.PrivateProtected,
            _ => AccessModifier.Internal
        };
    }
    public static string ToModifierString(AccessModifier? accessModifier)
    {
        return accessModifier switch
        {
            AccessModifier.Public => "public",
            AccessModifier.Private => "private",
            AccessModifier.Protected => "protected",
            AccessModifier.Internal => "internal",
            AccessModifier.ProtectedInternal => "protected internal",
            AccessModifier.PrivateProtected => "private protected",
            _ => string.Empty,
        };
    }
}

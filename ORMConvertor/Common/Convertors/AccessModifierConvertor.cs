using Model.AbstractRepresentation.Enums;

namespace Common.Convertors;
/// <summary>
/// Provides methods to convert between string representations and <see cref="AccessModifier"/> enum values.
/// </summary>
public static class AccessModifierConvertor
{
    /// <summary>
    /// Converts a string representation of an access modifier to its corresponding <see cref="AccessModifier"/> enum value.
    /// </summary>
    /// <param name="modifier">The string representation of the access modifier (public, private, …).</param>
    /// <returns>
    /// The corresponding <see cref="AccessModifier"/> value if the string matches a known modifier; 
    /// otherwise, returns <see cref="AccessModifier.Internal"/>.
    /// </returns>
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

    /// <summary>
    /// Converts an <see cref="AccessModifier"/> enum value to its string representation.
    /// </summary>
    /// <param name="accessModifier">The <see cref="AccessModifier"/> value to convert.</param>
    /// <returns>
    /// The string representation of the access modifier (public, private, …), 
    /// or an empty string if the value is null or not recognized.
    /// </returns>
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

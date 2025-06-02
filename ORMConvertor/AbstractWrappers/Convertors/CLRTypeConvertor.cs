using Model.AbstractRepresentation;
using Model.AbstractRepresentation.Enums;

namespace AbstractWrappers.Convertors;

public static class CLRTypeConvertor
{
    public static CLRType FromString(string? clrType)
    {
        if (string.IsNullOrWhiteSpace(clrType))
        {
            throw new ArgumentNullException(nameof(clrType));
        }

        return clrType.ToLowerInvariant() switch
        {
            "bool" => CLRType.Bool,
            "int" => CLRType.Int,
            "long" => CLRType.Long,
            "byte" => CLRType.Byte,
            "decimal" => CLRType.Decimal,
            "double" => CLRType.Double,
            "float" => CLRType.Float,
            "string" => CLRType.String,
            "char" => CLRType.Char,
            "datetime" => CLRType.DateTime,
            "object" => CLRType.Object,
            "list" => CLRType.List,
            _ => throw new NotSupportedException(clrType)
        };
    }

    public static string ToString(CLRType clrType)
    {
        return clrType switch
        {
            CLRType.Bool => "bool",
            CLRType.Byte => "byte",
            CLRType.Char => "char",
            CLRType.Int => "int",
            CLRType.Long => "long",
            CLRType.Double => "double",
            CLRType.Float => "float",
            CLRType.Decimal => "decimal",
            CLRType.String => "string",
            CLRType.DateTime => "DateTime",
            CLRType.Object => "object",
            CLRType.List => "List",
            _ => throw new NotSupportedException(clrType.ToString())
        };
    }

    public static string ToString(CLRTypeModel clrTypeModel)
    {
        if (clrTypeModel.GenericParam != null)
        {
            return $"{ToString(clrTypeModel.CLRType)}<{clrTypeModel.GenericParam}>";
        }
        return ToString(clrTypeModel.CLRType);
    }
}

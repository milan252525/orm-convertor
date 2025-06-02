using Model.AbstractRepresentation.Enums;

namespace EFCoreWrappers.Convertors;

public class DatabaseTypeConvertor
{
    public static DatabaseType FromEfCore(string? columnTypeOrClr)
    {
        if (string.IsNullOrWhiteSpace(columnTypeOrClr))
        {
            throw new ArgumentNullException(nameof(columnTypeOrClr));
        }

        // Strip precision/scale/length
        var type = columnTypeOrClr.Trim();
        var pos = type.IndexOf('(');
        if (pos >= 0)
        {
            type = type[..pos];
        }

        var sql = type.ToLowerInvariant();

        return sql switch
        {
            "bigint" => DatabaseType.BigInt,
            "int" or "integer" => DatabaseType.Int,
            "smallint" => DatabaseType.SmallInt,
            "tinyint" => DatabaseType.TinyInt,
            "bit" => DatabaseType.Bit,

            "decimal" => DatabaseType.Decimal,
            "numeric" => DatabaseType.Numeric,
            "money" => DatabaseType.Money,
            "smallmoney" => DatabaseType.SmallMoney,

            "float" => DatabaseType.Float,
            "real" => DatabaseType.Real,

            "date" => DatabaseType.Date,
            "datetime" => DatabaseType.DateTime,
            "datetime2" => DatabaseType.DateTime2,
            "smalldatetime" => DatabaseType.SmallDateTime,
            "time" => DatabaseType.Time,
            "datetimeoffset" => DatabaseType.DateTimeOffset,

            "char" => DatabaseType.Char,
            "varchar" => DatabaseType.VarChar,
            "text" => DatabaseType.Text,
            "nchar" => DatabaseType.NChar,
            "nvarchar" => DatabaseType.NVarChar,
            "ntext" => DatabaseType.NText,

            "binary" => DatabaseType.Binary,
            "varbinary" => DatabaseType.VarBinary,
            "image" => DatabaseType.Image,

            "uniqueidentifier" or "uuid"
                                => DatabaseType.UniqueIdentifier,
            "xml" => DatabaseType.Xml,
            "sql_variant" => DatabaseType.SqlVariant,
            "rowversion" or "timestamp"
                                => DatabaseType.RowVersion,

            // CLR type fall-back
            "long" => DatabaseType.BigInt,
            "int32" or "int" => DatabaseType.Int,
            "int16" or "short" => DatabaseType.SmallInt,
            "byte" => DatabaseType.TinyInt,
            "bool" or "boolean" => DatabaseType.Bit,

            "decimal" or "system.decimal" => DatabaseType.Decimal,
            "double" => DatabaseType.Float,
            "single" or "float" => DatabaseType.Real,

            "system.datetime" => DatabaseType.DateTime,
            "system.timespan" or "timespan" => DatabaseType.Time,
            "system.datetimeoffset" => DatabaseType.DateTimeOffset,

            "guid" or "system.guid" => DatabaseType.UniqueIdentifier,

            _ => throw new NotImplementedException(columnTypeOrClr)
        };
    }

    public static string ToEFCore(DatabaseType type) => type switch
    {
        DatabaseType.BigInt => "bigint",
        DatabaseType.Int => "int",
        DatabaseType.SmallInt => "smallint",
        DatabaseType.TinyInt => "tinyint",
        DatabaseType.Bit => "bit",

        DatabaseType.Decimal => "decimal",
        DatabaseType.Numeric => "numeric",
        DatabaseType.Money => "money",
        DatabaseType.SmallMoney => "smallmoney",

        DatabaseType.Float => "float",
        DatabaseType.Real => "real",

        DatabaseType.Date => "date",
        DatabaseType.DateTime => "datetime",
        DatabaseType.DateTime2 => "datetime2",
        DatabaseType.SmallDateTime => "smalldatetime",
        DatabaseType.Time => "time",
        DatabaseType.DateTimeOffset => "datetimeoffset",

        DatabaseType.Char => "char",
        DatabaseType.VarChar => "varchar",
        DatabaseType.Text => "text",
        DatabaseType.NChar => "nchar",
        DatabaseType.NVarChar => "nvarchar",
        DatabaseType.NText => "ntext",

        DatabaseType.Binary => "binary",
        DatabaseType.VarBinary => "varbinary",
        DatabaseType.Image => "image",

        DatabaseType.UniqueIdentifier => "uniqueidentifier",
        DatabaseType.Xml => "xml",
        DatabaseType.SqlVariant => "sql_variant",
        DatabaseType.RowVersion => "rowversion",

        _ => throw new NotImplementedException(type.ToString())
    };
}

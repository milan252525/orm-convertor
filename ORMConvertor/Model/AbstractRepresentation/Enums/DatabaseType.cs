namespace Model.AbstractRepresentation.Enums;

public enum DatabaseType
{
    None = 0,

    // Numeric Types
    BigInt = 1,
    Int = 2,
    SmallInt = 3,
    TinyInt = 4,
    Bit = 5,
    Decimal = 6,
    Numeric = 7,
    Money = 8,
    SmallMoney = 9,
    Float = 10,
    Real = 11,

    // Date and Time Types
    Date = 12,
    DateTime = 13,
    DateTime2 = 14,
    SmallDateTime = 15,
    Time = 16,
    DateTimeOffset = 17,

    // Character Types
    Char = 18,
    VarChar = 19,
    Text = 20,
    NChar = 21,
    NVarChar = 22,
    NText = 23,

    // Binary Types
    Binary = 24,
    VarBinary = 25,
    Image = 26,

    // Other Common Types
    UniqueIdentifier = 27,
    Xml = 28,
    SqlVariant = 29,
    RowVersion = 30
}

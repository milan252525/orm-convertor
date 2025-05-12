namespace Model.AbstractRepresentation.Enums;

public enum PrimaryKeyStrategy
{
    None,
    Increment,
    Identity,
    Sequence,
    HiLo,
    Uuid,
    Guid,
}

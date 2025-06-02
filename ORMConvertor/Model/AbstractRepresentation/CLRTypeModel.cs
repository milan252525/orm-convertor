using Model.AbstractRepresentation.Enums;

namespace Model.AbstractRepresentation;

public class CLRTypeModel
{
    public required CLRType CLRType { get; set; }

    public string? GenericParam { get; set; }
}

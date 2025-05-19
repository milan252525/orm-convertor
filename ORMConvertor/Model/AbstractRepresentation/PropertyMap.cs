namespace Model.AbstractRepresentation;

public class PropertyMap
{
    public required Property Property { get; set; }

    public string? ColumnName { get; set; }

    public string? Type { get; set; }

    public int? Precision { get; set; }

    public int? Scale { get; set; }

    public int? Length { get; set; }

    public bool? IsNullable { get; set; }

    public Dictionary<string, string> OtherDatabaseProperties { get; set; } = [];

    public Relation? Relation { get; set; } = null;
}

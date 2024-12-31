
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using RepoDb.Attributes;
using RepoDb.Interfaces;
using RepoDb.Options;

namespace RepoDBEntities;

[Table("People", Schema = "Application")]
public class Person
{
    public int PersonID { get; set; }

    public required string FullName { get; set; }

    public required string PreferredName { get; set; }

    public string? EmailAddress { get; set; }

    [PropertyHandler(typeof(PersonCustomFieldsPropertyHandler))]
    public CustomFields? CustomFields { get; set; }

    [PropertyHandler(typeof(PersonOtherLanguagesPropertyHandler))]
    public List<string>? OtherLanguages { get; set; }
}

public class CustomFields
{
    public List<string>? OtherLanguages { get; set; }
    public DateTime? HireDate { get; set; }
    public string? Title { get; set; }
}

public class PersonCustomFieldsPropertyHandler : IPropertyHandler<string, CustomFields?>
{
    public CustomFields? Get(string input, PropertyHandlerGetOptions options) =>
        !string.IsNullOrEmpty(input) ? JsonSerializer.Deserialize<CustomFields>(input) : null;

    public string Set(CustomFields? input, PropertyHandlerSetOptions options) =>
        input != null ? JsonSerializer.Serialize(input) : string.Empty;
}

public class PersonOtherLanguagesPropertyHandler : IPropertyHandler<string, List<string>?>
{
    public List<string>? Get(string input, PropertyHandlerGetOptions options) =>
        !string.IsNullOrEmpty(input) ? JsonSerializer.Deserialize<List<string>>(input) : null;

    public string Set(List<string>? input, PropertyHandlerSetOptions options) =>
        input != null ? JsonSerializer.Serialize(input) : string.Empty;
}


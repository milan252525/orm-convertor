using AbstractWrappers.Convertors;
using Model;
using Model.AbstractRepresentation;
using Model.AbstractRepresentation.Enums;

namespace AbstractWrappers;

public abstract class AbstractEntityBuilder
{
    public EntityMap EntityMap { get; set; } = new() { Entity = new() };

    public void AddTable(string tableName)
    {
        EntityMap.Table = tableName;
    }

    public void AddSchema(string schemaName)
    {
        EntityMap.Schema = schemaName;
    }

    public void AddClassHeader(string accessModifier, string className)
    {
        EntityMap.Entity.Name = className;

        // TODO Move away
        EntityMap.Entity.AccessModifier = AccessModifierConvertor.FromString(accessModifier);
    }

    public void AddPrimaryKey(PrimaryKeyStrategy strategy, string propertyName)
    {

    }

    public void AddForeignKey(Cardinality cardinality, string propertyName)
    {
    }

    public void AddProperty(
        string type,
        string propertyName,
        Dictionary<string, string> databaseProperties,
        string? accessModifier = null,
        List<string>? OtherModifiers = null
    )
    {
        EntityMap.Entity.Properties.Add(
            new Property
            {
                Name = propertyName,
                Type = type,
                AccessModifier = AccessModifierConvertor.FromString(accessModifier),
                OtherModifiers = OtherModifiers ?? []
            }
        );
    }

    public abstract ConversionResult Build();

    protected abstract void BuildImports();

    protected abstract void BuildTableSchema();

    protected abstract void BuildPrimaryKey();

    protected abstract void BuildForeignKey();

    protected abstract void BuildProperties();

    protected abstract void FinalizeBuild();
}

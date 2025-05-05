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

    public void AddNamespace(string namespaceName)
    {
        EntityMap.Entity.Namespace = namespaceName;
    }

    public void AddClassHeader(string accessModifier, string className)
    {
        EntityMap.Entity.Name = className;

        EntityMap.Entity.AccessModifier = AccessModifierConvertor.FromString(accessModifier.Trim());
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
        List<string>? OtherModifiers = null,
        bool hasGetter = false,
        bool hasSetter = false,
        string? defaultValue = null,
        bool isNullable = false
    )
    {
        var property = new Property
        {
            Name = propertyName,
            Type = type,
            AccessModifier = AccessModifierConvertor.FromString(accessModifier),
            OtherModifiers = OtherModifiers ?? [],
            HasGetter = hasGetter,
            HasSetter = hasSetter,
            DefaultValue = defaultValue,
            IsNullable = isNullable,
        };

        EntityMap.Entity.Properties.Add(property);

        EntityMap.PropertyMaps.Add(
            new PropertyMap
            {
                Property = property,
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

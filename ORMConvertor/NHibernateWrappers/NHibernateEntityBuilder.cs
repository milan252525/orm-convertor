using AbstractWrappers;
using AbstractWrappers.Convertors;
using Model;
using Model.AbstractRepresentation;
using Model.AbstractRepresentation.Enums;
using NHibernateWrappers.Convertors;
using System.Text;

namespace NHibernateWrappers;

public class NHibernateEntityBuilder : AbstractEntityBuilder
{
    private readonly StringBuilder codeResult = new();
    private readonly StringBuilder mappingResult = new();

    private bool classOpened;


    /// <summary>
    /// Builds the entity representation and its mapping.
    /// Entity is C# class, mapping is XML.
    /// Other types of mapping are not supported for now.
    /// </summary>
    public override List<ConversionSource> Build()
    {
        BuildImports();
        BuildTableSchema();
        BuildPrimaryKey();
        BuildProperties();
        BuildForeignKey();
        FinalizeBuild();

        return
        [
            new() { ContentType = ContentType.CSharp, Content = codeResult.ToString() },
            new() { ContentType = ContentType.XML, Content = mappingResult.ToString() }
        ];
    }

    /// <summary>
    /// Adds C# namespace.
    /// Adds XML prolog and root <hibernate-mapping> tag.
    /// </summary>
    protected override void BuildImports()
    {
        // No imports needed for NHibernate entity
        if (!string.IsNullOrWhiteSpace(EntityMap.Entity.Namespace))
        {
            codeResult.AppendLine($"namespace {EntityMap.Entity.Namespace};");
            codeResult.AppendLine();
        }

        // XML: prolog + root <hibernate-mapping>
        AppendXml(0, "<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
        var xmlNs = "urn:nhibernate-mapping-2.2";
        var nsAttr = string.IsNullOrWhiteSpace(EntityMap.Entity.Namespace)
            ? string.Empty
            : $" namespace=\"{EntityMap.Entity.Namespace}\"";
        AppendXml(0, $"<hibernate-mapping xmlns=\"{xmlNs}\"{nsAttr}>");
    }

    /// <summary>
    /// Builds C# class header and XML <class> tag.
    /// </summary>
    protected override void BuildTableSchema()
    {
        var modifier = AccessModifierConvertor.ToModifierString(EntityMap.Entity.AccessModifier);
        var name = EntityMap.Entity.Name;

        // C#
        codeResult.AppendLine($"{modifier} class {name}");
        codeResult.AppendLine("{");

        // XML <class>
        var nameWithNamespace = string.IsNullOrWhiteSpace(EntityMap.Entity.Namespace)
            ? name
            : $"{EntityMap.Entity.Namespace}.{name}, {EntityMap.Entity.Namespace}";

        var table = EntityMap.Table ?? name; // default = class name
        var schema = EntityMap.Schema ?? string.Empty; // TODO schema
        var schemaAttr = string.IsNullOrWhiteSpace(schema) ? string.Empty : $" schema=\"{schema}\"";

        AppendXml(1, $"<class name=\"{nameWithNamespace}\" table=\"{table}\"{schemaAttr}>");
        classOpened = true;
    }

    /// <summary>
    /// Builds C# primary key property and XML <id> tag.
    /// </summary>
    protected override void BuildPrimaryKey()
    {
        var primaryKeyPropertyMap = EntityMap.PropertyMaps.FirstOrDefault(pm =>
            pm.OtherDatabaseProperties.TryGetValue("IsPrimaryKey", out var v) &&
            string.Equals(v, "true", StringComparison.OrdinalIgnoreCase));

        if (primaryKeyPropertyMap is null)
        {
            return; // no PK
        }

        var prop = primaryKeyPropertyMap.Property;
        var columnName = primaryKeyPropertyMap.ColumnName ?? prop.Name;
        string nhType;
        if (primaryKeyPropertyMap.Type != null)
        {
            nhType = DatabaseTypeConvertor.ToNHibernate(primaryKeyPropertyMap.Type.Value);
        }
        else
        {
            // TODO this would be a place to query database for the missing type
            // for now we guess it from CLR type
            nhType = DatabaseTypeConvertor.GuessFromPropertyType(prop.Type.CLRType);
        }

        var generatorClass = primaryKeyPropertyMap.OtherDatabaseProperties.TryGetValue("PrimaryKeyStrategy", out var s) && int.TryParse(s, out var intVal)
            ? PrimaryKeyStrategyConvertor.ToNHibernate((PrimaryKeyStrategy)intVal)
            : "identity"; // default generator (TODO)

        AppendPropertyToCode(prop, isPrimaryKey: true);

        AppendXml(2, $"<id name=\"{prop.Name}\" column=\"{columnName}\" type=\"{nhType}\">");
        AppendXml(3, $"<generator class=\"{generatorClass}\" />");
        AppendXml(2, "</id>");
    }

    /// <summary>
    /// Builds C# properties and XML <property> tags.
    /// Primary and foreign keys are handled separately.
    /// </summary>
    protected override void BuildProperties()
    {
        foreach (var pm in EntityMap.PropertyMaps)
        {
            if (pm.OtherDatabaseProperties.TryGetValue("IsPrimaryKey", out var v) && v.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                continue; // handled in BuildPrimaryKey
            }

            if (pm.OtherDatabaseProperties.TryGetValue("IsForeignKey", out var fk) && fk.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                continue; // navigation property – handled in BuildForeignKey
            }

            AppendPropertyToCode(pm.Property);
            AppendPropertyToXml(pm);
        }
    }

    /// <summary>
    /// Builds C# foreign key properties and XML <one-to-one>, <many-to-one>, <bag> or <many-to-many> tags.
    /// </summary>
    protected override void BuildForeignKey()
    {
        // 1:1 and N:1 foreign keys
        foreach (var foreignKeyPropertyMap in EntityMap.PropertyMaps.Where(p => p.OtherDatabaseProperties.TryGetValue("IsForeignKey", out var v) && v.Equals("true", StringComparison.OrdinalIgnoreCase)))
        {
            var rel = foreignKeyPropertyMap.Relation; // single relation per FK property

            var xmlTag = rel?.Cardinality switch
            {
                Cardinality.OneToOne => "one-to-one",
                Cardinality.ManyToOne => "many-to-one",
                _ => null
            };

            if (xmlTag is null)
            {
                continue; // collection handled below
            }

            AppendPropertyToCode(foreignKeyPropertyMap.Property); // navigation property in C#

            var columnName = foreignKeyPropertyMap.ColumnName ?? foreignKeyPropertyMap.Property.Name;
            AppendXml(2, $"<{xmlTag} name=\"{foreignKeyPropertyMap.Property.Name}\" class=\"{rel!.Target}\" column=\"{columnName}\" />");
        }

        // 1:N and N:N collections
        foreach (var propertyMap in EntityMap.PropertyMaps.Where(p => p.Relation?.Cardinality is Cardinality.OneToMany or Cardinality.ManyToMany))
        {
            var relation = propertyMap.Relation!;

            // C# collection type - List<target>
            var targetShortName = relation.Target.Contains('.')
                ? relation.Target[(relation.Target.LastIndexOf('.') + 1)..]
                : relation.Target;

            var collectionType = $"List<{targetShortName}>";

            codeResult.AppendLine($"    {BuildPropertySignature(propertyMap.Property)}");
            codeResult.AppendLine();

            // XML <bag> (TODO: allow set/list/map etc.)
            // TODO other collection properties
            AppendXml(2, $"<bag name=\"{propertyMap.Property.Name}\" inverse=\"true\" cascade=\"all-delete-orphan\">");
            var primaryKeyCol = GetPrimaryKeyColumn();
            AppendXml(3, $"<key column=\"{primaryKeyCol}\" />");

            if (relation.Cardinality == Cardinality.OneToMany)
            {
                AppendXml(3, $"<one-to-many class=\"{relation.Target}\" />");
            }
            else // ManyToMany
            {
                AppendXml(3, $"<many-to-many class=\"{relation.Target}\" />");
            }

            AppendXml(2, "</bag>");
        }
    }

    /// <summary>
    /// Finalizes the build process by closing the class and XML tags.
    /// </summary>
    protected override void FinalizeBuild()
    {
        codeResult.AppendLine("}");

        if (classOpened)
        {
            AppendXml(1, "</class>");
        }

        AppendXml(0, "</hibernate-mapping>", appendLine: false);
    }

    /// <summary>
    /// Appends a property to the C# code.
    /// </summary>
    private void AppendPropertyToCode(Property prop, bool isPrimaryKey = false)
    {
        var declaration = BuildPropertySignature(prop, isPrimaryKey);
        codeResult.AppendLine($"    {declaration}");
        codeResult.AppendLine();
    }

    /// <summary>
    /// Appends a property to the XML mapping.
    /// </summary>
    private void AppendPropertyToXml(PropertyMap propertyMap)
    {
        var prop = propertyMap.Property;

        var attrs = new List<string> { $"name=\"{prop.Name}\"" };

        if (!string.IsNullOrWhiteSpace(propertyMap.ColumnName))
        {
            attrs.Add($"column=\"{propertyMap.ColumnName}\"");
        }

        if (propertyMap.IsNullable.HasValue)
        {
            attrs.Add($"not-null=\"{(!propertyMap.IsNullable.Value).ToString().ToLowerInvariant()}\"");
        }
        else if (!prop.IsNullable)
        {
            attrs.Add("not-null=\"true\"");
        }

        if (propertyMap.Type.HasValue)
        {
            attrs.Add($"type=\"{DatabaseTypeConvertor.ToNHibernate(propertyMap.Type.Value)}\"");
        }

        if (propertyMap.Precision.HasValue)
        {
            attrs.Add($"precision=\"{propertyMap.Precision.Value}\"");
        }

        if (propertyMap.Scale.HasValue)
        {
            attrs.Add($"scale=\"{propertyMap.Scale.Value}\"");
        }

        if (propertyMap.Length.HasValue)
        {
            attrs.Add($"length=\"{propertyMap.Length.Value}\"");
        }

        AppendXml(2, $"<property {string.Join(' ', attrs)} />");
    }

    /// <summary>
    /// Gets the primary key column name.
    /// </summary>
    private string GetPrimaryKeyColumn()
    {
        var pkMap = EntityMap.PropertyMaps.FirstOrDefault(pm =>
            pm.OtherDatabaseProperties.TryGetValue("IsPrimaryKey", out var v) && v.Equals("true", StringComparison.OrdinalIgnoreCase));
        return pkMap?.ColumnName ?? pkMap?.Property.Name ?? "Id";
    }

    /// <summary>
    /// Appends a line to the XML mapping with indentation.
    /// </summary>
    private void AppendXml(int indentLevels, string content, bool appendLine = true)
    {
        var indent = new string(' ', indentLevels * 4);
        if (appendLine)
        {
            mappingResult.AppendLine($"{indent}{content}");
        }
        else
        {
            mappingResult.Append($"{indent}{content}");
        }

    }

    /// <summary>
    /// Builds the property signature for C# code.
    /// Adds modifiers, type, name, getter/setter, and default value.
    /// </summary>
    private static string BuildPropertySignature(Property prop, bool isPrimaryKey = false)
    {
        var otherMods = new List<string>(prop.OtherModifiers ?? []);
        if (!otherMods.Any(m => m.Equals("virtual", StringComparison.OrdinalIgnoreCase)))
        {
            otherMods.Add("virtual");
        }

        var access = AccessModifierConvertor.ToModifierString(prop.AccessModifier);
        var modifiers = $"{access} {string.Join(' ', otherMods)}".Trim();
        var clrType = CLRTypeConvertor.ToString(prop.Type);
        var type = (!isPrimaryKey && prop.IsNullable) ? $"{clrType}?" : clrType;

        var getterSetter = (prop.HasGetter || prop.HasSetter)
            ? $" {{ {(prop.HasGetter ? "get;" : "")}{(prop.HasSetter ? " set;" : "")} }}"
            : "";
        var defaultVal = string.IsNullOrWhiteSpace(prop.DefaultValue)
            ? ""
            : $" = {prop.DefaultValue};";

        return $"{modifiers} {type} {prop.Name}{getterSetter}{defaultVal}";
    }
}

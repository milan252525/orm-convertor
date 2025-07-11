using AbstractWrappers;
using Common.Convertors;
using EFCoreWrappers.Convertors;
using Model;
using Model.AbstractRepresentation;
using System.Data;
using System.Text;

namespace EFCoreWrappers;

public class EFCoreEntityBuilder : AbstractEntityBuilder
{
    private readonly StringBuilder codeResult = new();

    /// <summary>
    /// Builds the entity representation and its mapping.
    /// </summary>
    public override List<ConversionSource> Build()
    {
        BuildImports();
        BuildTableSchema();
        BuildPrimaryKey();
        BuildProperties();
        BuildForeignKey();
        FinalizeBuild();

        return [
            new ConversionSource
            {
                ContentType = ConversionContentType.CSharpEntity,
                Content = codeResult.ToString()
            }
        ];
    }

    protected override void BuildForeignKey()
    {
        var foreignKeyPropertyMaps = EntityMap.PropertyMaps
            .Where(pm => pm.OtherDatabaseProperties.TryGetValue("IsForeignKey", out var v) &&
                     string.Equals(v, "true", StringComparison.OrdinalIgnoreCase));

        foreach (var propertyMap in foreignKeyPropertyMaps)
        {
            var rel = propertyMap.Relation;
            bool nullable = propertyMap.IsNullable ?? true;

            codeResult.Append(BuildPropertyAttributes(propertyMap));
            codeResult.AppendLine($"    {BuildPropertySignature(propertyMap.Property, nullable: nullable)}");
            codeResult.AppendLine();
        }
    }

    /// <summary>
    /// Adds namespace if present and imports required for EF Core entity mapping.
    /// </summary>
    protected override void BuildImports()
    {
        if (EntityMap.Entity.Namespace != null)
        {
            codeResult.AppendLine($"namespace {EntityMap.Entity.Namespace};");
            codeResult.AppendLine();
        }

        codeResult.AppendLine("using System.ComponentModel.DataAnnotations;");
        codeResult.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");

        codeResult.AppendLine();
    }

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

        // TODO primary key strategy

        bool nullable = primaryKeyPropertyMap.IsNullable ?? false;

        codeResult.Append(BuildPropertyAttributes(primaryKeyPropertyMap, isPrimaryKey: true));
        codeResult.AppendLine($"    {BuildPropertySignature(prop, isPrimaryKey: true, nullable: nullable)}");
        codeResult.AppendLine();
    }

    /// <summary>
    /// Builds the properties of the entity.
    /// </summary>
    protected override void BuildProperties()
    {
        foreach (var propertyMap in EntityMap.PropertyMaps)
        {
            if (propertyMap.OtherDatabaseProperties.TryGetValue("IsPrimaryKey", out var v) && v.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                continue; // handled in BuildPrimaryKey
            }

            if (propertyMap.OtherDatabaseProperties.TryGetValue("IsForeignKey", out var fk) && fk.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                continue; // navigation property – handled in BuildForeignKey
            }

            bool nullable = propertyMap.IsNullable ?? false;

            codeResult.Append(BuildPropertyAttributes(propertyMap));
            codeResult.AppendLine($"    {BuildPropertySignature(propertyMap.Property, nullable: nullable)}");
            codeResult.AppendLine();
        }
    }

    /// <summary>
    /// Builds table schema - attributes and class definition.
    /// </summary>
    protected override void BuildTableSchema()
    {
        if (EntityMap.Table != null)
        {
            string schemaIfPresent = "";
            if (EntityMap.Schema != null)
            {
                schemaIfPresent = $", Schema = \"{EntityMap.Schema}\"";
            }

            codeResult.AppendLine($"[Table(\"{EntityMap.Table}\"{schemaIfPresent})]");
        }

        var modifier = AccessModifierConvertor.ToModifierString(EntityMap.Entity.AccessModifier);
        var name = EntityMap.Entity.Name;

        codeResult.AppendLine($"{modifier} class {name}");
        codeResult.AppendLine("{");
    }

    /// <summary>
    /// Finalizes the build process by closing the class definition.
    /// </summary>
    protected override void FinalizeBuild()
    {
        codeResult.AppendLine("}");
    }

    /// <summary>
    /// Builds the property signature for C# code.
    /// Adds modifiers, type, name, getter/setter, and default value.
    /// </summary>
    private static string BuildPropertySignature(Property prop, bool isPrimaryKey = false, bool nullable = false)
    {
        var otherMods = new List<string>(prop.OtherModifiers ?? []);

        if (!nullable && !otherMods.Contains("required") && string.IsNullOrEmpty(prop.DefaultValue))
        {
            otherMods.Add("required");
        }

        var access = AccessModifierConvertor.ToModifierString(prop.AccessModifier);
        var modifiers = $"{access} {string.Join(' ', otherMods)}".Trim();

        var dbType = CLRTypeConvertor.ToString(prop.Type);
        var type = (!isPrimaryKey && prop.IsNullable) ? $"{dbType}?" : dbType;

        var getterSetter = (prop.HasGetter || prop.HasSetter)
            ? $" {{ {(prop.HasGetter ? "get;" : "")}{(prop.HasSetter ? " set;" : "")} }}"
            : "";
        var defaultVal = string.IsNullOrWhiteSpace(prop.DefaultValue)
            ? ""
            : $" = {prop.DefaultValue};";

        return $"{modifiers} {type} {prop.Name}{getterSetter}{defaultVal}";
    }

    /// <summary>
    /// Builds the property attributes for EF Core.
    /// </summary>
    private static string BuildPropertyAttributes(PropertyMap propMap, bool isPrimaryKey = false)
    {
        StringBuilder attributes = new();

        if (isPrimaryKey)
        {
            attributes.AppendLine($"    [Key]");
        }

        // This would be a place to query database type in the future, if needed
        if (propMap.ColumnName != null || propMap.Type != null)
        {
            var parts = new List<string>();
            if (propMap.ColumnName != null)
            {
                parts.Add($"\"{propMap.ColumnName}\"");
            }

            if (propMap.Type.HasValue)
            {
                var typeText = DatabaseTypeConvertor.ToEFCore(propMap.Type.Value);
                parts.Add($"TypeName=\"{typeText}\"");
            }

            attributes.AppendLine($"    [Column({string.Join(", ", parts)})]");
        }


        if (propMap.Length != null)
        {
            attributes.AppendLine($"    [MaxLength({propMap.Length})]");
        }

        if (propMap.Precision != null)
        {
            var args = propMap.Scale != null
                ? $"{propMap.Precision}, {propMap.Scale}"
                : $"{propMap.Precision}";

            attributes.AppendLine($"    [Precision({args})]");
        }

        return attributes.ToString();
    }
}

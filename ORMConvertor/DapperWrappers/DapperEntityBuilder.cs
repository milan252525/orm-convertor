using AbstractWrappers;
using AbstractWrappers.Convertors;
using Model;
using Model.AbstractRepresentation;
using System.Text;

namespace DapperWrappers;

public class DapperEntityBuilder : AbstractEntityBuilder
{
    private readonly StringBuilder codeResult = new();

    /// <summary>
    /// Builds the entity representation and its mapping.
    /// </summary>
    /// <returns></returns>
    public override ConversionResult Build()
    {
        BuildImports();
        BuildTableSchema();
        BuildProperties();
        FinalizeBuild();

        return new ConversionResult
        {
            ContentType = ContentType.CSharp,
            Content = codeResult.ToString()
        };
    }

    /// <summary>
    /// Dapper does not support foreign keys.
    /// </summary>
    protected override void BuildForeignKey()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Dapper needs no imports for the entity. Only builds namespace if provided.
    /// </summary>
    protected override void BuildImports()
    {
        if (EntityMap.Entity.Namespace != null)
        {
            codeResult.AppendLine($"namespace {EntityMap.Entity.Namespace};");
            codeResult.AppendLine();
        }

        // no imports for Dapper
    }

    /// <summary>
    /// Dapper does not support primary keys.
    /// </summary>
    protected override void BuildPrimaryKey()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Builds the properties of the entity.
    /// </summary>
    protected override void BuildProperties()
    {
        foreach (var property in EntityMap.Entity.Properties)
        {
            var modifiers = $"{AccessModifierConvertor.ToModifierString(property.AccessModifier)} {string.Join(' ', property.OtherModifiers)}".Trim();
            var type = property.IsNullable ? $"{property.Type}?" : property.Type;

            var getterSetter = (property.HasGetter || property.HasSetter)
                ? $" {{ {(property.HasGetter ? "get;" : string.Empty)}{(property.HasSetter ? "set;" : string.Empty)} }}"
                : string.Empty;

            var defaultValue = string.IsNullOrWhiteSpace(property.DefaultValue)
                ? string.Empty
                : $" = {property.DefaultValue};";

            codeResult.AppendLine($"\t{modifiers} {type} {property.Name}{getterSetter}{defaultValue}");
            codeResult.AppendLine();
        }
    }

    /// <summary>
    /// Dapped support no information about table schema.
    /// Only builds the class.
    /// </summary>
    protected override void BuildTableSchema()
    {
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
        codeResult.AppendLine();
    }
}

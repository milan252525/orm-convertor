using AbstractWrappers;
using AbstractWrappers.Convertors;
using Model;
using Model.AbstractRepresentation;
using System.Text;

namespace DapperWrappers;

public class DapperEntityBuilder : AbstractEntityBuilder
{
    private readonly StringBuilder codeResult = new();

    public override ConversionResult Build()
    {
        BuildImports();
        BuildTableSchema();
        BuildProperties();
        //BuildPrimaryKey();
        //BuildForeignKey();
        FinalizeBuild();

        return new ConversionResult
        {
            ContentType = ContentType.CSharp,
            Content = codeResult.ToString()
        };
    }

    protected override void BuildForeignKey()
    {
        throw new NotImplementedException();
    }

    protected override void BuildImports()
    {
        if (EntityMap.Entity.Namespace != null)
        {
            codeResult.AppendLine($"namespace {EntityMap.Entity.Namespace};");
            codeResult.AppendLine();
        }

        // no imports for Dapper
    }

    protected override void BuildPrimaryKey()
    {
        throw new NotImplementedException();
    }

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

    protected override void BuildTableSchema()
    {
        var modifier = AccessModifierConvertor.ToModifierString(EntityMap.Entity.AccessModifier);
        var name = EntityMap.Entity.Name;

        codeResult.AppendLine($"{modifier} class {name}");
        codeResult.AppendLine("{");
    }

    protected override void FinalizeBuild()
    {
        codeResult.AppendLine("}");
        codeResult.AppendLine();
    }
}

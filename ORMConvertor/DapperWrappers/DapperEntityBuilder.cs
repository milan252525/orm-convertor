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
            var modifier = AccessModifierConvertor.ToModifierString(property.AccessModifier);
            var otherModifiers = string.Join(' ', property.OtherModifiers).Trim();

            var type = property.Type;
            if (property.IsNullable ?? false)
            {
                type += "?";
            }

            var name = property.Name;

            var modifiers = string.IsNullOrWhiteSpace(otherModifiers)
                ? modifier
                : $"{modifier} {otherModifiers}";

            codeResult.Append($"\t{modifiers} {type} {name}");

            if (property.HasGetter || property.HasSetter)
            {
                var getter = property.HasGetter ? "get;" : string.Empty;
                var setter = property.HasGetter ? "set;" : string.Empty;
                codeResult.Append($" {{ {getter}{setter} }}");
            }

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

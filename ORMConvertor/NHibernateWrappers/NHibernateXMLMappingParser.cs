using AbstractWrappers;
using Model.AbstractRepresentation.Enums;
using NHibernateWrappers.Convertors;
using System.Xml.Linq;

namespace NHibernateWrappers;

/// <summary>
/// Parses NHibernate mapping from XML file.
/// Uses LINQ to XML to parse the mapping and extract relevant information.
/// </summary>
public class NHibernateXMLMappingParser(AbstractEntityBuilder entityBuilder) : IParser
{
    /// <summary>
    /// Parses an NHibernate mapping XML file from the provided source code string.
    /// </summary>
    /// <param name="source">String containing XML mapping file</param>
    public void Parse(string source)
    {
        var xmlDoc = XDocument.Parse(source);
        var mapping = xmlDoc.Root;
        if (mapping == null || mapping.Name.LocalName != "hibernate-mapping")
        {
            return;
        }

        ParseMapping(mapping);
    }

    private void ParseMapping(XElement mapping)
    {
        // Mapping-level namespace (attribute)
        var mappingNamespace = mapping.Attribute("namespace")?.Value;
        if (!string.IsNullOrEmpty(mappingNamespace))
        {
            entityBuilder.AddNamespace(mappingNamespace);
        }

        // Class parsing
        var classElement = mapping.Elements().FirstOrDefault(e => e.Name.LocalName == "class");
        if (classElement != null)
        {
            ParseClass(classElement);
        }
    }

    private void ParseClass(XElement classElement)
    {
        // Header (class name)
        var nameAttr = classElement.Attribute("name")?.Value;
        if (!string.IsNullOrEmpty(nameAttr))
        {
            var fullType = nameAttr.Split(',')[0].Trim();
            var className = fullType.Contains('.')
                ? fullType[(fullType.LastIndexOf('.') + 1)..]
                : fullType;

            entityBuilder.AddClassHeader(string.Empty, className);
        }

        // Table and schema
        var table = classElement.Attribute("table")?.Value;
        if (!string.IsNullOrEmpty(table))
        {
            entityBuilder.AddTable(table);
        }

        var schema = classElement.Attribute("schema")?.Value;
        if (!string.IsNullOrEmpty(schema))
        {
            entityBuilder.AddSchema(schema);
        }

        ParsePrimaryKey(classElement);
        ParseProperties(classElement);
        ParseRelations(classElement);
    }

    private void ParsePrimaryKey(XElement classElement)
    {
        var idElem = classElement.Elements().FirstOrDefault(e => e.Name.LocalName == "id");
        if (idElem == null)
        {
            return;
        }

        var propName = idElem.Attribute("name")?.Value;
        var generatorElem = idElem.Elements().FirstOrDefault(e => e.Name.LocalName == "generator");
        var genClass = generatorElem?.Attribute("class")?.Value;
        var strategy = PrimaryKeyStrategyConvertor.FromNHibernate(genClass);

        if (!string.IsNullOrEmpty(propName))
        {
            entityBuilder.AddPrimaryKey(strategy, propName);
        }
    }

    private void ParseProperties(XElement classElement)
    {
        foreach (var prop in classElement.Elements().Where(e => e.Name.LocalName == "property"))
        {
            var propertyName = prop.Attribute("name")?.Value;
            if (string.IsNullOrEmpty(propertyName))
            {
                continue;
            }

            var dbProps = new Dictionary<string, string>();
            if (prop.Attribute("column")?.Value is string col && !string.IsNullOrEmpty(col))
            {
                dbProps["column"] = col;
            }

            if (prop.Attribute("type")?.Value is string t && !string.IsNullOrEmpty(t))
            {
                dbProps["type"] = t;
            }

            var notNullAttr = prop.Attribute("not-null")?.Value;
            bool isNullable = notNullAttr == "false";

            dbProps["nullable"] = isNullable.ToString().ToLowerInvariant();

            entityBuilder.SetPropertyDatabaseMapping(
                propertyName,
                dbProps
            );
        }
    }

    private void ParseRelations(XElement classElement)
    {
        foreach (var relation in classElement.Elements().Where(e =>
             e.Name.LocalName == "one-to-one" ||
             e.Name.LocalName == "many-to-one"))
        {
            var propName = relation.Attribute("name")?.Value;
            if (string.IsNullOrEmpty(propName))
            {
                continue;
            }

            var cardinality = relation.Name.LocalName switch
            {
                "one-to-one" => Cardinality.OneToOne,
                "many-to-one" => Cardinality.ManyToOne,
                _ => throw new InvalidOperationException()
            };

            entityBuilder.AddForeignKey(cardinality, propName);
        }

        string[] collectionTypes = ["bag", "set", "list", "map"];
        foreach (var collection in classElement.Elements().Where(e => collectionTypes.Contains(e.Name.LocalName)))
        {
            var propName = collection.Attribute("name")?.Value;
            if (string.IsNullOrEmpty(propName))
            {
                continue;
            }

            if (collection.Elements().Any(e => e.Name.LocalName == "one-to-many"))
            {
                entityBuilder.AddForeignKey(Cardinality.OneToMany, propName);
            }
            else if (collection.Elements().Any(e => e.Name.LocalName == "many-to-many"))
            {
                entityBuilder.AddForeignKey(Cardinality.ManyToMany, propName);
            }

            // register a OneToMany foreign‐key/relation
            entityBuilder.AddForeignKey(Cardinality.OneToMany, propName);
        }
    }
}
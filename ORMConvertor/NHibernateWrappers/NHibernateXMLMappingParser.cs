using AbstractWrappers;
using Model;
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
    public bool CanParse(ContentType contentType)
    {
        return contentType == ContentType.XML;
    }

    /// <summary>
    /// Parses an NHibernate mapping XML file from the provided source code string.
    /// </summary>
    /// <param name="source">String containing XML mapping file</param>
    public void Parse(string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return;
        }

        var xmlDoc = XDocument.Parse(source.Trim());
        var mapping = xmlDoc.Root;
        if (mapping == null || mapping.Name.LocalName != "hibernate-mapping")
        {
            return;
        }

        ParseMapping(mapping);
    }

    /// <summary>
    /// Parses the mapping element of the NHibernate XML mapping file.
    /// </summary>
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

    /// <summary>
    /// Parses the class element of the NHibernate XML mapping file.
    /// </summary>
    private void ParseClass(XElement classElement)
    {
        // Header (class name)
        //var nameAttr = classElement.Attribute("name")?.Value;
        //if (!string.IsNullOrEmpty(nameAttr))
        //{
        //    var fullType = nameAttr.Split(',')[0].Trim();
        //    var className = fullType.Contains('.')
        //        ? fullType[(fullType.LastIndexOf('.') + 1)..]
        //        : fullType;

        //    entityBuilder.AddClassHeader(string.Empty, className);
        //}

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

    /// <summary>
    /// Parses the primary key element.
    /// </summary>
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

    /// <summary>
    /// Parses the properties of the class element, extracting their names, types, and database attributes.
    /// </summary>
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

            if (bool.TryParse(prop.Attribute("not-null")?.Value, out var notNull))
            {
                dbProps["nullable"] = (!notNull).ToString().ToLowerInvariant();
            }

            if (prop.Attribute("precision")?.Value is string prec && !string.IsNullOrWhiteSpace(prec))
            {
                dbProps["precision"] = prec;
            }

            if (prop.Attribute("scale")?.Value is string sc && !string.IsNullOrWhiteSpace(sc))
            {
                dbProps["scale"] = sc;
            }

            if (prop.Attribute("length")?.Value is string len && !string.IsNullOrWhiteSpace(len))
            {
                dbProps["length"] = len;
            }

            entityBuilder.SetPropertyDatabaseMapping(
                propertyName,
                dbProps
            );
        }
    }

    /// <summary>
    /// Parses the relations defined in the class element, such as one-to-one, many-to-one, one-to-many, and many-to-many.
    /// </summary>
    private void ParseRelations(XElement classElement)
    {
        foreach (var relation in classElement.Elements().Where(e =>
             e.Name.LocalName == "one-to-one" ||
             e.Name.LocalName == "many-to-one"))
        {
            var propName = relation.Attribute("name")?.Value;
            var target = relation.Attribute("class")?.Value;
            if (string.IsNullOrEmpty(propName) || string.IsNullOrEmpty(target))
            {
                continue;
            }

            var cardinality = relation.Name.LocalName switch
            {
                "one-to-one" => Cardinality.OneToOne,
                "many-to-one" => Cardinality.ManyToOne,
                _ => throw new InvalidOperationException()
            };

            entityBuilder.AddForeignKey(cardinality, propName, target);
        }

        string[] collectionTypes = ["bag", "set", "list", "map"];
        foreach (var collection in classElement.Elements().Where(e => collectionTypes.Contains(e.Name.LocalName)))
        {
            var propName = collection.Attribute("name")?.Value;
            if (string.IsNullOrEmpty(propName))
            {
                continue;
            }

            var oneToMany = collection.Elements().FirstOrDefault(e => e.Name.LocalName == "one-to-many");
            if (oneToMany != null)
            {
                var target = oneToMany.Attribute("class")?.Value;
                if (!string.IsNullOrEmpty(target))
                {
                    entityBuilder.AddForeignKey(Cardinality.OneToMany, propName, target);
                }
                continue;
            }

            var manyToMany = collection.Elements().FirstOrDefault(e => e.Name.LocalName == "many-to-many");
            if (manyToMany != null)
            {
                var target = manyToMany.Attribute("class")?.Value;
                if (!string.IsNullOrEmpty(target))
                {
                    entityBuilder.AddForeignKey(Cardinality.ManyToMany, propName, target);
                }
            }
        }
    }
}
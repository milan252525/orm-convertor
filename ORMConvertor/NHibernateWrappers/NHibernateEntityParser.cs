﻿using AbstractWrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Model;
using Model.AbstractRepresentation;
using NHibernateWrappers.Convertors;

namespace NHibernateWrappers;

/// <summary>
/// Parses a C# class definition (optionally within a namespace) from the provided source code string.
/// </summary>
public class NHibernateEntityParser(AbstractEntityBuilder entityBuilder) : IParser
{
    public bool CanParse(ConversionContentType contentType)
    {
        return contentType == ConversionContentType.CSharpEntity;
    }

    /// <summary>
    /// Parses a C# class definition (optionally within a namespace) from the provided source code string.
    /// </summary>
    /// <param name="source">C# source code containing a single class, optionally wrapped in a namespace.</param>
    public void Parse(string source)
    {
        var root = CSharpSyntaxTree.ParseText(source).GetCompilationUnitRoot();

        var ns = root.Members.OfType<BaseNamespaceDeclarationSyntax>().FirstOrDefault();

        // Supporting single class per source for now
        var cls = root.DescendantNodes()
                  .OfType<ClassDeclarationSyntax>()
                  .First();

        if (ns is not null)
        {
            ParseNamespace(ns);
        }
        ParseClassHeader(cls);
        ParseProperties(cls);
    }

    /// <summary>
    /// Parses the namespace declaration.
    /// </summary>
    private void ParseNamespace(BaseNamespaceDeclarationSyntax namespaceDeclaration)
    {
        entityBuilder.AddNamespace(namespaceDeclaration.Name.ToString());
    }

    /// <summary>
    /// Parses the class header, including modifiers and class name.
    /// </summary>
    private void ParseClassHeader(ClassDeclarationSyntax classDeclaration)
    {
        var modifiers = string.Join(" ", classDeclaration.Modifiers.Select(m => m.Text));

        entityBuilder.AddClassHeader(
            modifiers,
            classDeclaration.Identifier.Text
        );
    }

    /// <summary>
    /// Parses the properties of the class, extracting their types, names, access modifiers, and other attributes.
    /// </summary>
    private void ParseProperties(ClassDeclarationSyntax classDeclaration)
    {
        foreach (var prop in classDeclaration.Members.OfType<PropertyDeclarationSyntax>())
        {
            var name = prop.Identifier.Text;
            var accessTokens = prop.Modifiers
                .Where(m =>
                    m.IsKind(SyntaxKind.PublicKeyword) ||
                    m.IsKind(SyntaxKind.PrivateKeyword) ||
                    m.IsKind(SyntaxKind.InternalKeyword) ||
                    m.IsKind(SyntaxKind.ProtectedKeyword))
                .Select(t => t.Text)
                .ToList();
            var accessModifiers = string.Join(" ", accessTokens);

            var otherModifiers = prop.Modifiers
                        .Where(m => !accessTokens.Contains(m.Text))
                        .Select(m => m.Text)
                        .ToList();

            bool hasGetter = prop.ExpressionBody != null
                    || prop.AccessorList?.Accessors.Any(a => a.IsKind(SyntaxKind.GetAccessorDeclaration)) == true;

            bool hasSetter = prop.AccessorList?.Accessors
                        .Any(a => a.IsKind(SyntaxKind.SetAccessorDeclaration)) == true;

            bool isNullable = prop.Type is NullableTypeSyntax;
            string type = ((prop.Type as NullableTypeSyntax)?.ElementType ?? prop.Type).ToString();

            var defaultValue = prop.Initializer?.Value?.ToString();

            entityBuilder.AddProperty(
                type,
                name,
                accessModifiers,
                otherModifiers,
                hasGetter: hasGetter,
                hasSetter: hasSetter,
                defaultValue: defaultValue,
                isNullable: isNullable
            );
        }
    }
}

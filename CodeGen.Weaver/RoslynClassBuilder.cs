using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeGen.Common.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.Weaver
{
    public class RoslynClassBuilder
    {
        public List<MethodDeclarationSyntax> Methods = new List<MethodDeclarationSyntax>();
        public List<PropertyDeclarationSyntax> Properties = new List<PropertyDeclarationSyntax>();
        public AttributeListSyntax Attributes { get; set; }
        public string Extends { get; set; }
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public List<string> Usings { get; set; } = new List<string>();

        public RoslynClassBuilder(string nameSpace, string name, string extends, AttributeListSyntax attributes)
        {
            NameSpace = nameSpace;
            Name = name;
            Extends = extends;
            Attributes = attributes;
        }

        public RoslynClassBuilder AddUsings(List<string> usings)
        {
            Usings.AddRange(usings);
            return this;
        }

        public string Build()
        {
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(NameSpace)).NormalizeWhitespace();

            Usings.ForEach(@using => @namespace =
                @namespace = @namespace.AddUsings(
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(@using)))
            );

            var classDeclaration =
                SyntaxFactory.ClassDeclaration(Name)
                    .AddModifiers(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddBaseListTypes(
                        SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(Extends)));

            Properties.ForEach(property => classDeclaration = classDeclaration.AddMembers(property));
            Methods.ForEach(method => classDeclaration = classDeclaration.AddMembers(method));

            classDeclaration =
                classDeclaration.AddAttributeLists(Attributes);

            // Add the class to the namespace.
            @namespace = @namespace.AddMembers(classDeclaration);

            return @namespace
                .NormalizeWhitespace()
                .ToFullString();
        }

        public RoslynClassBuilder AddProperty(string name, string typeName, object initialValue, SchemaPropertyAccessors accessors, AttributeListSyntax attributes)
        {
            var propertyDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(typeName), name)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            var accessorList = new List<AccessorDeclarationSyntax>();

            if (accessors.HasFlag(SchemaPropertyAccessors.Get))
            {
                accessorList.Add(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                );
            }

            if (accessors.HasFlag(SchemaPropertyAccessors.Set))
            {
                accessorList.Add(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                );
            }

            propertyDeclaration = propertyDeclaration
                .AddAccessorListAccessors(
                    accessorList.ToArray()
                );

            if (initialValue != null)
            {
                SyntaxToken literalSyntaxToken;
                switch (initialValue)
                {
                    case string value:
                        literalSyntaxToken = SyntaxFactory.Literal(value);
                        break;

                    case int value:
                        literalSyntaxToken = SyntaxFactory.Literal(value);
                        break;

                    default:
                        //TODO not sure what we do about non primitive types here?
                        throw new ArgumentOutOfRangeException($"Unsupported initial property value type {initialValue.GetType()}");

                }

                propertyDeclaration = propertyDeclaration
                    .WithInitializer(
                        SyntaxFactory.EqualsValueClause(
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                literalSyntaxToken)))
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
            }


            if (attributes?.ChildNodes()?.Count() > 0)
            {
                propertyDeclaration = propertyDeclaration.AddAttributeLists(attributes);
            }

            Properties.Add(propertyDeclaration);
            return this;
        }


        public RoslynClassBuilder AddMethod(string name, string returns, string code, AttributeListSyntax attributes)
        {
            var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(returns), name)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            var methodStatements = new List<StatementSyntax>();
            if (string.IsNullOrWhiteSpace(code) == false)
            {
                methodStatements.Add(SyntaxFactory.ParseStatement(code));
            }

            methodDeclaration = methodDeclaration.WithBody(SyntaxFactory.Block(
                methodStatements
            ));

            if (attributes?.ChildNodes()?.Count() > 0)
            {
                methodDeclaration = methodDeclaration.AddAttributeLists(attributes);
            }

            Methods.Add(methodDeclaration);
            return this;
        }
    }
}
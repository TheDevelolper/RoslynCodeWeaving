using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using CodeGen.Common.Contracts;
using CodeGen.Common.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.Weaver
{
    public class RoslynNetWriter : IClassWriter
    {
        public string Write(SchemaClass @class)
        {

            var classAttributeBuilder = new RoslynAttributesBuilder();

            foreach (var schemaAttribute in @class.Attributes)
            {
                classAttributeBuilder.AddAttribute(name: schemaAttribute.Name, schemaAttribute.Arguments);
            }

            var attributes =
                classAttributeBuilder.BuildAttributes();

            var classBuilder = new RoslynClassBuilder(
                    nameSpace: @class.NameSpace,
                    name: @class.Name,
                    extends: @class.Extends,
                    attributes: attributes);

            classBuilder
                .AddUsings(@class.Usings.ToList());


            @class.Methods.ToList().ForEach(
                schemaMethod =>
                {
                    var attributesBuilder = new RoslynAttributesBuilder();
                    schemaMethod.Attributes.ToList().ForEach(a =>
                        attributesBuilder.AddAttribute(a.Name, a.Arguments)
                    );

                    classBuilder.AddMethod(
                        schemaMethod.Name,
                        schemaMethod.ReturnTypeName,
                        schemaMethod.CodeContents,
                        attributesBuilder.BuildAttributes()
                    );
                }
            );


            @class.Properties.ToList().ForEach(
                schemaProperty =>
                {
                    var attributesBuilder = new RoslynAttributesBuilder();
                    schemaProperty.Attributes.ToList().ForEach(a =>
                        attributesBuilder.AddAttribute(a.Name, a.Arguments)
                    );

                    classBuilder.AddProperty(
                        schemaProperty.Name,
                        schemaProperty.TypeName,
                        "SomeString",
                        schemaProperty.Accessors,
                        attributesBuilder.BuildAttributes()
                    );
                }
            );

            return classBuilder.Build();
        }

        public class RoslynAttributesBuilder
        {
            private SyntaxList<AttributeSyntax> attributeList = new SyntaxList<AttributeSyntax>();

            public RoslynAttributesBuilder AddAttribute(string name, ICollection<string> arguments = null)
            {
                AttributeSyntax attribute;
                if (arguments?.Count > 0)
                {
                    var argumentsList = CreateRoslynAttributeArguments(arguments);
                    attribute = SyntaxFactory.Attribute(
                        SyntaxFactory.IdentifierName(name),
                        SyntaxFactory.AttributeArgumentList(argumentsList));
                }
                else
                {
                    attribute = SyntaxFactory.Attribute(
                        SyntaxFactory.IdentifierName(name));
                }

                attributeList = attributeList.Add(attribute);

                return this;
            }

            private static SeparatedSyntaxList<AttributeArgumentSyntax> CreateRoslynAttributeArguments(IEnumerable<string> arguments)
            {
                var roslynArgs = new SeparatedSyntaxList<AttributeArgumentSyntax>();
                foreach (var argument in arguments)
                {
                    roslynArgs = roslynArgs.Add(
                        SyntaxFactory.AttributeArgument(
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(argument)))
                    );
                }

                return roslynArgs;
            }

            public AttributeListSyntax BuildAttributes() =>
                SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(attributeList));
        }

        public void Dispose() => GC.SuppressFinalize(this);
    }

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

            var MethodStatements = new List<StatementSyntax>();
            if (string.IsNullOrWhiteSpace(code) == false)
            {
                MethodStatements.Add(SyntaxFactory.ParseStatement(code));
            }

            methodDeclaration = methodDeclaration.WithBody(SyntaxFactory.Block(
                MethodStatements
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
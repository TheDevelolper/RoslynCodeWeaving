using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.Weaver
{
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
}
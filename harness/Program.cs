using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(CreateClass());
            Console.ReadLine();
        }

            static string CreateClass()
            {
                // Create a namespace: (namespace CodeGenerationSample)
                var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("WebHarness")).NormalizeWhitespace();

                // Add System using statement: (using System)
                @namespace = @namespace.AddUsings(
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Collections.Generic")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Threading.Tasks")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Microsoft.AspNetCore.Mvc")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Microsoft.Extensions.Logging"))
                    );

                //  Create a class: (class Order)
                var classDeclaration =
                    SyntaxFactory.ClassDeclaration("KiranController")

                .AddAttributeLists(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("ApiController")))
                ).NormalizeWhitespace())

                .AddAttributeLists(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("Route"),
                            SyntaxFactory.AttributeArgumentList(
                                SyntaxFactory.SeparatedList(new[]
                            {
                                SyntaxFactory.AttributeArgument(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(@"Sample")))

                            }))))))

                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                        .AddBaseListTypes(
                            SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("ControllerBase")));


                var syntax = SyntaxFactory.ParseStatement("return Content(\"Hello Wooooorld\");");

                // Create a method
                var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("IActionResult"), "Get")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))

                    .AddAttributeLists(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("HttpGet")
                            ))))
                    .WithBody(SyntaxFactory.Block(syntax));


                // Add the field, the property and method to the class.
                classDeclaration = classDeclaration.AddMembers(methodDeclaration);

                // Add the class to the namespace.
                @namespace = @namespace.AddMembers(classDeclaration);

                // Normalize and get code as string.
                var code = @namespace
                    .NormalizeWhitespace()
                    .ToFullString();

                // Output new code to the console.
                return code;
            }

    }
}

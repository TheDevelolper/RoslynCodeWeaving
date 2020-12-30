using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using CodeGen.Common.Contracts;
using CodeGen.Common.Models;

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

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
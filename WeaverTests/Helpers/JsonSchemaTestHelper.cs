using System;
using System.IO;
using System.Text.Json;

using CodeGen.Common.Models;
using CodeGenSchema = CodeGen.Common.Models.Schema;
using System.Collections.Generic;

namespace Tests.Helpers
{
    internal static class JsonSchemaTestHelper
    {
        public static CodeGenSchema CreateJsonSchema()
        {
            var schema = new CodeGenSchema
            {
                Solutions = new List<SchemaNetSolution>()
                {
                    new()
                    {
                        Projects = new List<SchemaNetProject>()
                        {
                            new()
                            {
                                Classes = new List<SchemaClass>()
                                {
                                    new()
                                    {
                                        Name = "TestController",
                                        NameSpace = "WebHarness",
                                        Extends = "ControllerBase",
                                        Usings = new List<string>()
                                        {
                                            "System",
                                            "Microsoft.AspNetCore.Mvc",
                                            "Microsoft.Extensions.Logging"
                                        },
                                        Attributes =
                                            new List<SchemaAttribute>()
                                            {
                                                new()
                                                {
                                                    Name = "ApiController"
                                                },
                                                new()
                                                {
                                                    Name = "Route",
                                                    Arguments = new List<string>() {"Sample"}
                                                }
                                            },
                                        Properties =
                                            new List<SchemaProperty>()
                                            {
                                                new()
                                                {
                                                    Name = "SomeString", 
                                                    TypeName = "string", 
                                                    InitialValue = "HelloWorld",
                                                    Accessors = SchemaPropertyAccessors.Get | SchemaPropertyAccessors.Set,
                                                },
                                            },
                                        Methods = new List<SchemaMethod>()
                                        {
                                            new()
                                            {
                                                Name = "SomeMethod",
                                                ReturnTypeName = "string",
                                                Attributes =
                                                    new List<SchemaAttribute>()
                                                    {
                                                        new()
                                                        {
                                                            Name = "HttpGet"
                                                        },
                                                    },
                                                CodeContents = "return $\"Hello {SomeString}\";"
                                                
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                }
            };
            return schema;
        }

        public static bool WriteJsonSchemaToFile(string filePath, CodeGenSchema schema)
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(schema));
            return File.Exists(filePath);
        }

    }
}

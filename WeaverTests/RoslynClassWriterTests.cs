using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CodeGen.Common.Contracts;
using CodeGen.Common.Models;
using CodeGen.Schema.Reader.Json;
using CodeGen.Weaver;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests
{
    public sealed class RoslynClassWriterTests: BaseTestClass<IClassWriter>
    {
        public Schema Schema { get; set; }

        private static readonly Regex WhitespaceRegex = new Regex(@"\s+");

        public static string ReplaceWhitespace(string input, string replacement)
        {
            return WhitespaceRegex.Replace(input, replacement);
        }


        public override void Setup()
        {
            Schema=
                JsonSchemaTestHelper.CreateJsonSchema();

            Sut = new RoslynNetWriter();
        }

        [Test]
        public void CreatesExpectedOutput()
        {
            var expectedOutput =  File.ReadAllText(PathHelper.DotNetExampleClass.FullName);

            var actualOutput = Sut.Write(
                Schema.
                    Solutions.
                    First().Projects.First().Classes.First());

            expectedOutput = ReplaceWhitespace(expectedOutput, string.Empty);
            actualOutput = ReplaceWhitespace(actualOutput, string.Empty);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

    }

}



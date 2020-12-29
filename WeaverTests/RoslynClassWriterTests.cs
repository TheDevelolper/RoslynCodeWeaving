using System;
using System.IO;
using System.Linq;
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

            Assert.AreEqual(expectedOutput, actualOutput);
        }

    }

}


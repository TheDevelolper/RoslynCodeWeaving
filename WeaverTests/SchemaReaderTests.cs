using System.IO;
using System.Linq;
using CodeGen.Common.Contracts;
using CodeGen.Schema.Reader.Json;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests
{
    public sealed class SchemaReaderTests : BaseTestClass<ISchemaReader>
    {
        public override void Setup()
        {
            var jsonFilePath = PathHelper.JsonSampleFile.FullName;
            var schema =
                JsonSchemaTestHelper.CreateJsonSchema();
          
            var fileWasCreated=
               JsonSchemaTestHelper.WriteJsonSchemaToFile(jsonFilePath, schema);

            Assert.IsTrue(fileWasCreated);

            Sut = new JsonFileSchemaReader(jsonFilePath);
        }

        [TearDown]
        public new void TearDown()
        {
            var jsonFilePath = PathHelper.JsonSampleFile.FullName;
            File.Delete(jsonFilePath);
            base.TearDown();
        }

        [Test]
        public void CanReadNetSolutions() => Assert.IsNotEmpty(Sut.Read().Solutions);

        [Test]
        public void CanReadProjects() => Assert.IsNotEmpty(Sut.Read().Solutions.First().Projects);

        [Test]
        public void CanReadClasses() => Assert.IsNotEmpty(Sut.Read().Solutions.First().Projects.First().Classes);

        [Test]
        public void CanReadClassAttributes() => Assert.IsNotEmpty(Sut.Read().Solutions.First().Projects.First().Classes.First().Attributes);

        [Test]
        public void CanReadProperties() => Assert.IsNotEmpty(Sut.Read().Solutions.First().Projects.First().Classes.First().Properties);

        [Test]
        public void CanGetPropertyType() => Assert.AreEqual(typeof(int).ToString(), Sut.Read().Solutions.First().Projects.First().Classes.First().Properties.First().TypeName);

        [Test]
        public void CanReadMethods()
        {
            var lastMethod = Sut.Read().Solutions.First().Projects.First().Classes.First().Methods.LastOrDefault();

            Assert.IsNotNull(lastMethod);
        }

        [Test]
        public void CanReadMethodReturnType()
        {
            var lastMethod = Sut.Read().Solutions.First().Projects.First().Classes.First().Methods.LastOrDefault();

            Assert.IsNotNull(lastMethod);
            Assert.IsNotNull(lastMethod.ReturnTypeName);
            Assert.AreEqual(lastMethod.ReturnTypeName, "string");
        }

        [Test]
        public void CanReadMethodAttributes()
        {
            var lastMethod = Sut.Read().Solutions.First().Projects.First().Classes.First().Methods.LastOrDefault();

            Assert.IsNotNull(lastMethod);
            Assert.IsNotEmpty(lastMethod.Attributes);
        }
    }
}
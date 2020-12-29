using System.Collections.Generic;

namespace CodeGen.Common.Models
{
    public class Schema
    {
        public ICollection<SchemaNetSolution> Solutions { get; set; }

    }

    public class SchemaNetSolution
    {
        public string Name { get; set; }
        public ICollection<SchemaNetProject> Projects { get; set; }
    }

    public class SchemaNetProject
    {
        public string Name { get; set; }
        public NetProjectTypes NetProjectType { get; set; }
        public ICollection<SchemaClass> Classes { get; set; } = new List<SchemaClass>();

    }

    public enum NetProjectTypes
    {
        ClassLibrary,
        WebApi,
        Console,
        UnitTest
    }
}

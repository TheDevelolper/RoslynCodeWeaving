using System.Collections.Generic;

namespace CodeGen.Common.Models
{
    public class SchemaClass
    {
        public string NameSpace { get; set; }
        public string Name { get; set; }
        public string Implements { get; set; } 
        public string Extends { get; set; }
        public ICollection<string> Usings { get; set; }

        public ICollection<SchemaAttribute> Attributes { get; set; } = new List<SchemaAttribute>();
        public ICollection<SchemaMethod> Methods { get; set; }       = new List<SchemaMethod>();
        public ICollection<SchemaProperty> Properties { get; set; } = new List<SchemaProperty>();
    }
}
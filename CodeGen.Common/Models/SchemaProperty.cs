using System.Collections.Generic;

namespace CodeGen.Common.Models
{
    public class SchemaProperty
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public object InitialValue { get; set; }
        public ICollection<SchemaAttribute> Attributes { get; set; } = new List<SchemaAttribute>();
    }
}
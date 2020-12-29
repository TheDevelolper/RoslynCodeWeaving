using System.Collections.Generic;

namespace CodeGen.Common.Models
{
    public class SchemaAttribute
    {
        public string Name { get; set; }
        public ICollection<string> Arguments { get; set; }
    }
}
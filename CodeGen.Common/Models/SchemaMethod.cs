using System.Collections.Generic;

namespace CodeGen.Common.Models
{
    public class SchemaMethod
    {
        public string Name { get; set; }
        public string ReturnTypeName { get; set; }
        public ICollection<SchemaAttribute> Attributes { get; set; }
        public string CodeContents { get; set; }
    }
}
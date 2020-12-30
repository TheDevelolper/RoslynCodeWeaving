using System;

namespace CodeGen.Common.Models
{
    [Flags]
    public enum SchemaPropertyAccessors : short
    {
        Get = 0,
        Set = 1
    }
}
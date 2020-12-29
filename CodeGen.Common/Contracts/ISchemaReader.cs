using System;

namespace CodeGen.Common.Contracts
{
    public interface ISchemaReader : IDisposable
    {
        Models.Schema Read();
    }
}

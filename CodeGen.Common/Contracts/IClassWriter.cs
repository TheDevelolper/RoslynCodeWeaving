using System;
using System.Threading.Tasks;
using CodeGen.Common.Models;

namespace CodeGen.Common.Contracts
{
    public interface IClassWriter : IDisposable
    {
        /// <summary>
        /// Adapts the specified schema and specifies files to be written.
        /// </summary>
        /// <param name="schema">The schema.</param>
        string Write(SchemaClass @class);
    }
}

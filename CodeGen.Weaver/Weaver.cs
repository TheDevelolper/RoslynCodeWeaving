using CodeGen.Common.Contracts;

namespace CodeGen.Weaver
{
    /// <summary>
    /// The weaver is responsible for taking schema and generating the classes into the target application.
    /// </summary>
    public class Weaver : IWeaver
    {
        internal Weaver(
            ISchemaReader importAdapter, 
            IClassWriter exportAdapter)
        {
            SchemaReader = importAdapter;
            Writer = exportAdapter;
        }

        public ISchemaReader SchemaReader { get; }
        public IClassWriter Writer { get; }
    }
    
}

namespace CodeGen.Common.Contracts
{
    public interface IWeaver
    {
        ISchemaReader SchemaReader { get; }
        IClassWriter Writer { get;  }
    }
}

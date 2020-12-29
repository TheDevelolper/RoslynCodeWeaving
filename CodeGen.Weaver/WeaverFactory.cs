using CodeGen.Common.Contracts;

namespace CodeGen.Weaver
{
    /// <summary>
    /// Creates Weavers
    /// </summary>
    public static class WeaverFactory
    {
        public static IWeaver Create(
            ISchemaReader importAdapter,
            IClassWriter exportAdapter) =>
             new Weaver(importAdapter, exportAdapter);
    }


}

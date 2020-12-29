using System;
using System.IO;
using System.Text.Json;
using CodeGen.Common.Contracts;

namespace CodeGen.Schema.Reader.Json
{
    public class JsonFileSchemaReader : ISchemaReader
    {
        public string FilePath { get; }

        public JsonFileSchemaReader(string filePath)
        {
            FilePath = filePath;
        }
        public Common.Models.Schema Read()
        {
            var contents = File.ReadAllText(FilePath);
            var result =
                JsonSerializer.Deserialize<Common.Models.Schema>(contents);
            return result;
        }

        public void Dispose() => GC.SuppressFinalize(this);
    }
}

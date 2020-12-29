using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Helpers
{
    internal static class PathHelper
    {
        public static DirectoryInfo WorkingDirectory => new(Environment.CurrentDirectory);

        public static FileInfo JsonSampleFile =>
            new(Path.Combine(WorkingDirectory.FullName, "Samples", "example.json"));

        public static FileInfo DotNetExampleClass =>
            new(Path.Combine(WorkingDirectory.FullName, "Samples", "TestController.cs"));

    }
}

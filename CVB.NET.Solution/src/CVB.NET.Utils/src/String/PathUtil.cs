namespace CVB.NET.Utils.String
{
    using System;
    using System.IO;
    using System.Reflection;
    using PostSharp.Patterns.Contracts;

    public static class PathUtil
    {
        public static string MapPhysicalPath([NotEmpty] string physicalRootPath, [NotEmpty] string virtualSubPath)
        {
            string physicalSubPath = virtualSubPath.Replace("/", "\\").EnsurePrefix("\\");

            return physicalRootPath.EnsureNoSuffix("\\") + physicalSubPath;
        }

        public static string GetAssemblyPath(Assembly assembly = null)
        {
            string codeBase = (assembly ?? Assembly.GetExecutingAssembly()).EscapedCodeBase;
            Uri uri = new Uri(codeBase);
            return Path.GetDirectoryName(uri.LocalPath).EnsureSuffix("\\");
        }
    }
}
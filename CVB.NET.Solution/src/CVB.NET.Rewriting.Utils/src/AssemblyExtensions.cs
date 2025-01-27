namespace CVB.NET.Rewriting.Utils
{
    using System;
    using System.Reflection;

    public static class AssemblyExtensions
    {
        public static string GetAssemblyLocalPath(this Assembly assembly)
        {
            if (assembly.EscapedCodeBase == null)
            {
                return null;
            }

            return new Uri(assembly.EscapedCodeBase).LocalPath.ToLowerInvariant();
        }

        public static string GetAssemblyLocalPath(this AssemblyName assembly)
        {
            if (assembly.EscapedCodeBase == null)
            {
                return null;
            }

            return new Uri(assembly.EscapedCodeBase).LocalPath.ToLowerInvariant();
        }
    }
}
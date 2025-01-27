using System.Reflection;

namespace CVB.NET.Rewriting.Compiler.Helpers
{
    public interface IAssemblyDependencyResolver
    {
        string ResolveAssemblySourcePath(AssemblyName assemblyName);
        Assembly ResolveAssembly(AssemblyName assemblyName);
    }
}

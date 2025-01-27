using System.Collections.Generic;
using Mono.Cecil;

namespace CVB.NET.Rewriting.Compiler.Services.Interfaces.Cecil
{
    public interface ICecilAssemblySearch
    {
        IEnumerable<IMethodUsage> FindMethodUsages(AssemblyDefinition targetAssembly, MethodReference method);
    }
}
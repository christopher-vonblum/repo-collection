using System.Reflection;

using Mono.Cecil;

namespace CVB.NET.Rewriting.Compiler.Services.Interfaces.Cecil
{
    public interface ICecilAssemblyTransformationContext
    {
        AssemblyDefinition GetTransformationAssembly(AssemblyName reflectionAssembly);

        void WriteTransformations();
    }
}
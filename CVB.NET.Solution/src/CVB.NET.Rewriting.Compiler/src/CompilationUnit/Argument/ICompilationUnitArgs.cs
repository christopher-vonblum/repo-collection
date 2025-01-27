using System.Reflection;

namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Argument
{
    public interface ICompilationUnitArgs
    {
        string PreTransformationPath { get; }
        string TransformationOutputPath { get; }
        string TransformationOutputAssembly { get; }
        AssemblyName AssemblyName { get; set; }
    }
}
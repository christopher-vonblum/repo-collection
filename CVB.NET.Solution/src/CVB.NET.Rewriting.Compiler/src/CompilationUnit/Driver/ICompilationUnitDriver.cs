using CVB.NET.Rewriting.Compiler.CompilationUnit.Argument;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Result;
using CVB.NET.Rewriting.Compiler.Configuration.Models.CompilationUnit;

namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Driver
{
    public interface ICompilationUnitDriver
    {
        ICompilationUnitResult ExecuteRecursive(ICompilationUnitArgs args, ICompilationUnitConfiguration configuration);
    }
}

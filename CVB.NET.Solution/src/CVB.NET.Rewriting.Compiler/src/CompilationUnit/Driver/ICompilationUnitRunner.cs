using CVB.NET.Rewriting.Compiler.CompilationUnit.Argument;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Result;
using CVB.NET.Rewriting.Compiler.Configuration.Models.CompilationUnit;

namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Driver
{
    public interface ICompilationUnitRunner
    {
        ICompilationUnitResult Evaluate(ICompilationUnitArgs args, ICompilationUnitConfiguration unitConfiguration);
    }
}
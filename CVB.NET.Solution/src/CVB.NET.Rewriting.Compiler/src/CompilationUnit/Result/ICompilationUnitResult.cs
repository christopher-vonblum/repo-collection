namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Result
{
    using Configuration.Models.CompilationUnit;
    using Error;

    public interface ICompilationUnitResult
    {
        ICompilationUnitConfiguration UnitConfiguration { get; }
        bool InitializationFailed { get; }
        bool BuildSucceeded { get; }
        ICompilationError[] CompilationErrors { get; }
        ICompilationUnitResult[] ChildResults { get; }
    }
}
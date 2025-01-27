namespace CVB.NET.Rewriting.Compiler.Result
{
    using CompilationUnit.Result;

    public interface ICompilationResult
    {
        ICompilationUnitResult[] UnitResults { get; set; }
        ICompilationUnitResult[] FailedUnitResults { get; }
        bool CompilationSucceeded { get; }
    }
}
namespace CVB.NET.Rewriting.Compiler.Result
{
    using CompilationUnit.Result;

    public class CompilationResult : ICompilationResult
    {
        public ICompilationUnitResult[] UnitResults { get; set; }
        public ICompilationUnitResult[] FailedUnitResults { get; set; }

        public bool CompilationSucceeded { get; set; }
    }
}
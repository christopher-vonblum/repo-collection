namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Result
{
    using System;
    using Configuration.Models.CompilationUnit;
    using Error;

    [Serializable]
    public class CompilationUnitResult : ICompilationUnitResult
    {
        public ICompilationUnitConfiguration UnitConfiguration { get; set; }

        public bool InitializationFailed { get; set; } = false;

        public bool BuildSucceeded { get; set; } = false;
        public ICompilationError[] CompilationErrors { get; set; } = new ICompilationError[0];
        public ICompilationUnitResult[] ChildResults { get; set; } = new ICompilationUnitResult[0];
    }
}
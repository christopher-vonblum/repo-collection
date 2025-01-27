namespace CVB.NET.Rewriting.Compiler.CompilationUnit
{
    using Argument;
    using Configuration.Models.CompilationUnit;
    using Result;

    public interface ICompilationUnit
    {
        ICompilationUnitConfiguration Configuration { get; }
        void Configure(ICompilationUnitConfiguration unitConfiguration);
        ICompilationUnitResult Invoke(ICompilationUnitArgs args);
    }
}
namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Factory
{
    using Configuration.Models.CompilationUnit;

    /// <summary>
    /// Exists once per isolation domain.
    /// Creates execution unit instances from execution unit configurations.
    /// Used by ICustomCompilationExecutionUnitRunner.
    /// </summary>
    public interface ICompilationUnitFactory
    {
        ICompilationUnit CreateUnit(ICompilationUnitConfiguration unitConfiguration);
    }
}
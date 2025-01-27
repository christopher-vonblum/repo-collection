namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Task
{
    using Configuration.Models.CompilationUnit.Task;

    public interface ICompilationTask : ICompilationUnit
    {
        new ICompilationTaskConfiguration Configuration { get; }
    }
}
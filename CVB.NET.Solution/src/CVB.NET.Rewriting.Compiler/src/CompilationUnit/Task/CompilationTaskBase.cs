namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Task
{
    using Argument;
    using Configuration.Models.CompilationUnit.Task;

    public abstract class CompilationTaskBase : CompilationTaskBase<ICompilationTaskConfiguration>
    {
    }

    public abstract class CompilationTaskBase<TTaskConfiguration> : CompilationUnitBase<TTaskConfiguration>, ICompilationTask
        where TTaskConfiguration : class, ICompilationTaskConfiguration
    { 
        ICompilationTaskConfiguration ICompilationTask.Configuration => Configuration;
    }
}

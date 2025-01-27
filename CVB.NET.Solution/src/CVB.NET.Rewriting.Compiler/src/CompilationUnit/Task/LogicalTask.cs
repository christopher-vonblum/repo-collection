namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Task
{
    using Argument;
    using Configuration.Models.CompilationUnit.Task;
    using Result;

    public class LogicalTask : CompilationTaskBase<ICompilationTaskConfiguration>
    {
        public override ICompilationUnitResult Execute(ICompilationUnitArgs args)
        {
            return CreateResult();
        }
    }
}
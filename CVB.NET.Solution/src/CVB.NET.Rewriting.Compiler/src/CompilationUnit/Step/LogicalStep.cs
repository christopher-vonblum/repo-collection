namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Step
{
    using Argument;
    using Configuration.Models.CompilationUnit.Step;
    using Result;

    public class LogicalStep : CompilationStepBase<ICompilationStepConfiguration>
    {
        public override ICompilationUnitResult Execute(ICompilationUnitArgs args)
        {
            return CreateResult();
        }
    }
}
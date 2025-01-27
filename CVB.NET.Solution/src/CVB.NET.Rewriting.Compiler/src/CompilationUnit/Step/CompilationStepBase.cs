namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Step
{
    using Argument;
    using Configuration.Models.CompilationUnit.Step;

    public abstract class CompilationStepBase : CompilationStepBase<ICompilationStepConfiguration>
    {
    }
    public abstract class CompilationStepBase<TStepConfiguration> : CompilationUnitBase<TStepConfiguration>, ICompilationStep
        where TStepConfiguration : class, ICompilationStepConfiguration
    {
        ICompilationStepConfiguration ICompilationStep.Configuration => Configuration;
    }
}
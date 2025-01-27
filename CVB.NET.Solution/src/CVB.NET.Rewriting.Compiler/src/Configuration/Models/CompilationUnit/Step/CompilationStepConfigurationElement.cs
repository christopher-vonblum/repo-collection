namespace CVB.NET.Rewriting.Compiler.Configuration.Models.CompilationUnit.Step
{
    using System;

    using Rewriting.Compiler.CompilationUnit.Step;

    [Serializable]
    public class CompilationStepConfigurationElement : CompilationUnitConfigurationBase, ICompilationStepConfiguration
    {
        public string[] DependsOnSteps { get; }
        public override Type ImplementationType => typeof (LogicalStep);
    }
}
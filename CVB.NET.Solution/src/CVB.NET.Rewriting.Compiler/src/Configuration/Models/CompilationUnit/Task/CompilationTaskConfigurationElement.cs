namespace CVB.NET.Rewriting.Compiler.Configuration.Models.CompilationUnit.Task
{
    using System;
    using Rewriting.Compiler.CompilationUnit.Task;

    [Serializable]
    public class CompilationTaskConfigurationElement : CompilationUnitConfigurationBase, ICompilationTaskConfiguration
    {
        public override Type ImplementationType => typeof(LogicalTask);
    }
}
namespace CVB.NET.Rewriting.Compiler.Configuration.Models.CompilationUnit.Task
{
    using Attributes;

    [ConfigurationInterfaceImplementation(typeof(CompilationTaskConfigurationElement))]
    public interface ICompilationTaskConfiguration : ICompilationUnitConfiguration
    {
    }
}
namespace CVB.NET.Rewriting.Compiler.Configuration.Models.CompilationUnit.Step
{
    using Attributes;

    [ConfigurationInterfaceImplementation(typeof(CompilationStepConfigurationElement))]
    public interface ICompilationStepConfiguration : ICompilationUnitConfiguration
    {
        string[] DependsOnSteps { get; }
    }
}
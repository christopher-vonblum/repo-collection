namespace CVB.NET.Rewriting.Compiler.Configuration
{
    using System.Reflection;

    using Models.CompilationUnit.Step;
    using Models.CompilationUnit.Task;

    public interface ICompilerConfigurationRepository
    {
        Assembly[] GetServiceAssemblies();
        ICompilationStepConfiguration[] GetAllStepConfigurations();
        ICompilationTaskConfiguration GetTaskConfiguration(string name);
    }
}
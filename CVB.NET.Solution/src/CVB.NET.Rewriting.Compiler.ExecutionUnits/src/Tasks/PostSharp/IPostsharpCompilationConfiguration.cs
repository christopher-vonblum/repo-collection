namespace CVB.NET.Rewriting.Compiler.CompilationUnits.Tasks.PostSharp
{
    using Configuration.Models.Attributes;
    using Configuration.Models.CompilationUnit.Task;

    [ConfigurationInterfaceImplementation(typeof(PostsharpCompilationTaskConfigurationElement))]
    public interface IPostsharpCompilationConfiguration : ICompilationTaskConfiguration
    {
        string BinaryDownloadUri { get; }
        string TargetBinary { get; }
    }
}

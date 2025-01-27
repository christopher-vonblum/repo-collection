namespace CVB.NET.Rewriting.Compiler.CompilationUnits.Tasks.PostSharp
{
    using Configuration.Models.CompilationUnit.Task;

    public class PostsharpCompilationTaskConfigurationElement : CompilationTaskConfigurationElement, IPostsharpCompilationConfiguration
    {
        public string BinaryDownloadUri { get; }
        public string TargetBinary { get; }
    }
}
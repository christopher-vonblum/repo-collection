namespace CVB.NET.Rewriting.Compiler.Services.Interfaces.Roslyn
{
    using Microsoft.CodeAnalysis;

    public interface IRoslynTransformationContext
    {
        Project Project { get; }
        Compilation Compilation { get; }
    }
}
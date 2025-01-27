namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Step
{
    using Configuration.Models.CompilationUnit.Step;

    // Reflection reloading needed for every step; isolate this in a appdomain
    public interface ICompilationStep : ICompilationUnit
    {
        new ICompilationStepConfiguration Configuration { get; }
    }
}
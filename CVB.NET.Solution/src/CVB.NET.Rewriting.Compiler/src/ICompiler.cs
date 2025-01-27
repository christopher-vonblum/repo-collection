namespace CVB.NET.Rewriting.Compiler
{
    using Argument;
    using Result;

    public interface ICompiler
    {
        ICompilationResult Compile(IPredefinedBuildArgs buildArgs);
    }
}
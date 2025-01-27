namespace CVB.NET.Rewriting.Compiler.Services.Interfaces.MsBuild
{
    public interface IMsBuildSolution
    {
        string SolutionDir { get; }
        IMsBuildProject LoadProject(string fullProjectPath);
    }
}
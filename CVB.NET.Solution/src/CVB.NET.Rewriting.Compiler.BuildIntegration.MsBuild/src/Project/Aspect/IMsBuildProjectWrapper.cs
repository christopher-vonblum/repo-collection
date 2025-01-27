namespace CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild.Project.Aspect
{
    using Project = Microsoft.Build.Evaluation.Project;

    [ProjectWrapperAspect]
    public interface IMsBuildProjectWrapper
    {
        Project InnerProject { get; }
    }
}
namespace CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild.Project.Aspect
{
    using Project = Microsoft.Build.Evaluation.Project;
    public abstract class MsBuildProjectWrapperBase : IMsBuildProjectWrapper
    {
        protected MsBuildProjectWrapperBase(Project innerProject)
        {
            InnerProject = innerProject;
        }

        public Project InnerProject { get; }

        public static bool operator ==(MsBuildProjectWrapperBase project, Project compare)
        {
            return project.InnerProject == compare;
        }

        public static bool operator !=(MsBuildProjectWrapperBase project, Project compare)
        {
            return project.InnerProject != compare;
        }
    }
}
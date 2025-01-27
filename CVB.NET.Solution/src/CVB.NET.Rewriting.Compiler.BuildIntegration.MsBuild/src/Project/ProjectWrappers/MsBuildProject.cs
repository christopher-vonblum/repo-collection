using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild.Project.Aspect;
using CVB.NET.Rewriting.Compiler.Services.Interfaces.MsBuild;
using Microsoft.Build.Evaluation;
using Unity.Attributes;

namespace CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild.Project.ProjectWrappers
{
    using Project = Microsoft.Build.Evaluation.Project;
    public class MsBuildProject : MsBuildProjectWrapperBase, IMsBuildProject
    {
        IMsBuildSolution IMsBuildProject.Solution => MsBuildSolution;
        public string ProjectDir { get; }
        public string IntermediateOutputPath { get; }

        [IgnoreCustomProperty]
        string IMsBuildProject.IntermediateOutputPath => ProjectDir + IntermediateOutputPath;

        public MsBuildSolution MsBuildSolution { get; }

        public Guid ProjectGuid => Guid.Parse(InnerProject.GetProperty("ProjectGuid").EvaluatedValue);

        public string Configuration { get; }

        public string Platform { get; }

        public string TargetFileName { get; }

        [IgnoreCustomProperty]
        public string IntermediateAssembly => ((IMsBuildProject)this).IntermediateOutputPath + TargetFileName;

        public string OutputPath { get; set; }
        [IgnoreCustomProperty]
        string IMsBuildProject.OutputPath => ProjectDir + OutputPath;

        [InjectionConstructor]
        public MsBuildProject(IMsBuildSolution solution, MsBuildArgs buildArgs) : this((MsBuildSolution)solution, ((IMsBuildProjectWrapper)solution.LoadProject(buildArgs.ProjectFile)).InnerProject)
        {
        }

        public MsBuildProject(MsBuildSolution msBuildSolution, Project innerProject) : base(innerProject)
        {
            MsBuildSolution = msBuildSolution;
        }

        public MsBuildProject(Project innerProject) : base(innerProject)
        {
        }

        public IMsBuildProject[] GetReferencedProjects()
        {
            Project[] referenceProjects = ResolveReferenceProjects(InnerProject.FullPath);

            if (MsBuildSolution == null)
            {
                throw new InvalidOperationException();
            }

            return referenceProjects.Select(proj => (IMsBuildProject)new MsBuildProject(MsBuildSolution, proj)).ToArray();
        }

        private Project[] ResolveReferenceProjects(string targetProject)
        {
            var dict = new Dictionary<string, Project>();

            ResolveReferenceProjects(targetProject, dict);

            dict.Remove(targetProject);

            return dict.Select(kv => kv.Value).ToArray();
        }

        private void ResolveReferenceProjects(string targetProject, Dictionary<string, Project> resolvedProjects)
        {
            Project outproj = ((IMsBuildProjectWrapper)MsBuildSolution.LoadProject(targetProject)).InnerProject;

            if (!resolvedProjects.ContainsKey(outproj.FullPath))
            {
                resolvedProjects[outproj.FullPath] = outproj;
            }

            ICollection<ProjectItem> projectReferences = outproj.GetItems("ProjectReference");

            string msbuildProjectFolder = Directory.GetParent(targetProject).FullName;

            foreach (ProjectItem projectReference in projectReferences)
            {
                string foreignProjectFile = msbuildProjectFolder + "\\" + projectReference.EvaluatedInclude;

                string foreignProjectPath = Path.GetFullPath(foreignProjectFile);

                ResolveReferenceProjects(foreignProjectPath, resolvedProjects);
            }
        }
    }
}
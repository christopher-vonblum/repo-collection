using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild.Project.Aspect;
using CVB.NET.Rewriting.Compiler.Services.Interfaces.MsBuild;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Evaluation;
using Unity.Attributes;

namespace CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild.Project.ProjectWrappers
{
    using Project = Microsoft.Build.Evaluation.Project;

    public class MsBuildSolution : MsBuildProjectWrapperBase, IMsBuildSolution
    {
        public string SolutionDir { get; }

        private IDictionary<Guid, string> ProjectConfigurations { get; } = new Dictionary<Guid, string>();

        private IDictionary<Guid, string> ProjectPlatforms { get; } = new Dictionary<Guid, string>();

        private ConcurrentDictionary<string, Guid> ProjectGuids { get; } = new ConcurrentDictionary<string, Guid>();

        private readonly string solutionPath;
        private readonly SolutionProjectLoader projectLoader;

        [InjectionConstructor]
        public MsBuildSolution(MsBuildArgs buildArgs) : this(buildArgs.SolutionFile, buildArgs.Configuration, buildArgs.Platform)
        {
        }

        public MsBuildSolution(string solutionPath, string solutionConfiguration, string solutionPlatform) : this(solutionPath, new Dictionary<string, string> { { "Configuration", solutionConfiguration }, { "Platform", solutionPlatform } })
        {

        }

        public MsBuildSolution(string solutionPath, IDictionary<string, string> globalProperties) : base(GetSolutionWrapperProject(solutionPath, globalProperties))
        {
            this.solutionPath = solutionPath;

            this.projectLoader = new SolutionProjectLoader();

            string solutionConfigurationXml = InnerProject.AllEvaluatedProperties.First(prop => prop.Name == "CurrentSolutionConfigurationContents").EvaluatedValue;

            XmlDocument configDoc = new XmlDocument();

            configDoc.Load(new MemoryStream(Encoding.Unicode.GetBytes(solutionConfigurationXml.ToCharArray())));

            IEnumerable<XmlElement> configElements = configDoc
                .DocumentElement
                .GetElementsByTagName("ProjectConfiguration")
                .OfType<XmlElement>();

            foreach (XmlElement projectConfigElement in configElements)
            {
                Guid projectGuid = Guid.Parse(projectConfigElement.GetAttribute("Project"));

                string[] splittedFlags = projectConfigElement.InnerText.Split('|');

                ProjectConfigurations[projectGuid] = splittedFlags[0];

                ProjectPlatforms[projectGuid] = splittedFlags[1];
            }
        }

        private static Project GetSolutionWrapperProject(string solutionPath, IDictionary<string, string> globalProperties)
        {
            string wrapperContent = SolutionWrapperProject.Generate(solutionPath, null, null);

            byte[] rawWrapperContent = Encoding.Unicode.GetBytes(wrapperContent.ToCharArray());

            using (MemoryStream memStream = new MemoryStream(rawWrapperContent))
            using (XmlTextReader xmlReader = new XmlTextReader(memStream))
            {
                return new ProjectCollection().LoadProject(
                    xmlReader,
                    globalProperties,
                    null);
            }
        }

        public IMsBuildProject LoadProject(string fullProjectPath)
        {
            Guid projectGuid = GetProjectGuid(fullProjectPath);

            return new MsBuildProject(
                            this,
                            projectLoader.LoadProjectAsync(
                                fullProjectPath,
                                new Dictionary<string, string>
                                {
                                    { "Configuration", GetProjectConfiguration(projectGuid) },
                                    { "Platform", GetProjectPlatform(projectGuid) }
                                }).Result);
        }

        private Guid GetProjectGuid(string projectFile)
        {
            return ProjectGuids.GetOrAdd(projectFile.ToLowerInvariant(),
                (project) =>
                    new MsBuildProject(new SolutionProjectLoader().LoadProjectAsync(project, null).Result).ProjectGuid);
        }

        public string GetProjectConfiguration(Guid projectGuid)
        {
            if (!ProjectConfigurations.ContainsKey(projectGuid))
            {
                throw new ProjectNotPartOfSolutionException(solutionPath, projectGuid);
            }

            return ProjectConfigurations[projectGuid];
        }

        public string GetProjectPlatform(Guid projectGuid)
        {
            if (!ProjectPlatforms.ContainsKey(projectGuid))
            {
                throw new ProjectNotPartOfSolutionException(solutionPath, projectGuid);
            }

            return ProjectPlatforms[projectGuid];
        }
    }

    public class ProjectNotPartOfSolutionException : Exception
    {
        public ProjectNotPartOfSolutionException(string solutionPath, Guid projectGuid) : base($@"Solution: ""{solutionPath}"", Project: ""{projectGuid}""")
        {
        }
    }
}
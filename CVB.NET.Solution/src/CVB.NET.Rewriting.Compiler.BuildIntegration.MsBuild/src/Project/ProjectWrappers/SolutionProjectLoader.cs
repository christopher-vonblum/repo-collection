using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild.Project.ProjectWrappers
{
    using Project = Microsoft.Build.Evaluation.Project;
    public class SolutionProjectLoader
    {
        private static readonly XmlReaderSettings xmlSettings = new XmlReaderSettings()
                                                                {
                                                                    DtdProcessing = DtdProcessing.Prohibit,
                                                                    XmlResolver = (XmlResolver)null
                                                                };

        private ProjectCollection solutionCollection;

        public SolutionProjectLoader()
        {
            solutionCollection = new ProjectCollection();
        }
        
        public async Task<Project> LoadProjectAsync(string projectPath, IDictionary<string, string> properties)
        {
            Project proj;
            if ((proj = solutionCollection.GetLoadedProjects(projectPath).SingleOrDefault()) != null)
            {
                return proj;
            }

            Dictionary<string, string> derivedProperties = new Dictionary<string, string>(properties ?? ImmutableDictionary<string, string>.Empty);

            derivedProperties["DesignTimeBuild"] = "true";
            derivedProperties["BuildingInsideVisualStudio"] = "true";

            using (XmlReader xmlReader = XmlReader.Create(await ReadFileAsync(projectPath).ConfigureAwait(false), xmlSettings))
            {
                ProjectRootElement projectXml = ProjectRootElement.Create(xmlReader, solutionCollection);

                projectXml.FullPath = projectPath;

                return new Project(projectXml, derivedProperties, null, solutionCollection);
            }
        }

        private static async Task<MemoryStream> ReadFileAsync(string path)
        {
            MemoryStream memoryStream = new MemoryStream();
            byte[] buffer = new byte[1024];
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous);
            try
            {
                int count;
                do
                {
                    count = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                    memoryStream.Write(buffer, 0, count);
                }
                while (count > 0);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
            memoryStream.Position = 0L;
            return memoryStream;
        }
    }
}
namespace CVB.NET.BuildIntegration.Tools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Microsoft.Build.Evaluation;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using NuGet;
    using Utils.String;

    public class NuspecGenerationTask : AppDomainIsolatedTask
    {
        private static readonly string[] libFileExtensions = new[] {".dll", ".exe", ".xml", ".pdb", ".config", ".pssym"};

        [Required]
        public string SolutionDirectory { get; set; }

        [Required]
        public string ProjectFile { get; set; }

        [Required]
        public string BuildOutputDir { get; set; }

        [Required]
        public string NugetFeedRootDir { get; set; }

        [Required]
        public string CurrentProjectConfiguration { get; set; }

        private string ProjectDirectory => Path.GetDirectoryName(ProjectFile);

        public override bool Execute()
        {
            if (!File.Exists(ProjectFile))
            {
                throw new FileNotFoundException(ProjectFile);
            }

            Project current = ProjectCollection.GlobalProjectCollection.LoadProject(ProjectFile);

            XDocument packageConfig = GetProjectPackagesConfig(current);

            current.Save();

            AddPackagesToPackagesConfig(current, packageConfig);

            string libDir = ProjectDirectory.EnsureSuffix("\\") + "lib\\";

            if (!Directory.Exists(libDir))
            {
                Directory.CreateDirectory(libDir);
            }

            string contentDir = ProjectDirectory.EnsureSuffix("\\") + "content\\";

            if (!Directory.Exists(contentDir))
            {
                Directory.CreateDirectory(contentDir);
            }

            string toolsDir = ProjectDirectory.EnsureSuffix("\\") + "tools\\";

            if (!Directory.Exists(toolsDir))
            {
                Directory.CreateDirectory(toolsDir);
            }

            CopyOutputFilesToProjectDirectory("lib", IsLibFile);
            CopyOutputFilesToProjectDirectory("tools", IsToolsFile);

            XDocument nuspecFile = GetProjectNuspecFile();

            RegisterFileDependencies(nuspecFile);

            AddReferencesToNuspecFile(current, nuspecFile);

            ProjectCollection.GlobalProjectCollection.UnloadAllProjects();

            SaveXDoc(packageConfig, ProjectDirectory.EnsureSuffix("\\") + "packages.config");

            SaveXDoc(nuspecFile, ProjectDirectory.EnsureSuffix("\\") + $"{Path.GetFileNameWithoutExtension(ProjectFile)}.nuspec");

            return true;
        }


        private XDocument GetProjectPackagesConfig(Project current)
        {
            string packagesConfigFileName = ProjectDirectory.EnsureSuffix("\\") + "packages.config";

            if (!File.Exists(packagesConfigFileName))
            {
                File.WriteAllText(packagesConfigFileName,
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
</packages>");
            }

            if (current.GetItems("None")
                .FirstOrDefault(item => item.UnevaluatedInclude.ToLowerInvariant() == "packages.config") == null)
            {
                current.AddItem("None", "packages.config");
            }

            return XDocument.Parse(File.ReadAllText(packagesConfigFileName));
        }

        private XDocument GetProjectNuspecFile()
        {
            string template =
                $@"<?xml version=""1.0""?>
<package>
  <metadata>
    <id>#ID#</id>
    <version>$version$</version>
    <authors>$author$</authors>
    <owners>$author$</owners>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>#ID#: $description$</description>
    <copyright>Copyright {DateTime.UtcNow.Year}</copyright>
    <references>$references$</references>
    <dependencies>
    </dependencies>
  </metadata>
</package>";

            string nuspecFilePath = ProjectDirectory.EnsureSuffix("\\") +
                                 $"{Path.GetFileNameWithoutExtension(ProjectFile)}.nuspec";

            if (!File.Exists(nuspecFilePath))
            {
                File.WriteAllText(nuspecFilePath, template.Replace("#ID#", Path.GetFileNameWithoutExtension(nuspecFilePath)));
            }

            return XDocument.Parse(File.ReadAllText(nuspecFilePath));
        }

        private void AddPackagesToPackagesConfig(Project msbuildProject, XDocument packageConfig)
        {
            ICollection<ProjectItem> orderedReferences = msbuildProject.GetItems("ProjectReference");

            IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository(NugetFeedRootDir.EnsureSuffix("\\") + CurrentProjectConfiguration.EnsureSuffix("\\"));

            foreach (ProjectItem referencedProjectNode in orderedReferences)
            {
                string fileName = Path.GetFileNameWithoutExtension(referencedProjectNode.EvaluatedInclude);

                IEnumerable<IPackage> packages = repo.FindPackagesById(fileName);

                IPackage version = packages.OrderBy(p => p.IsLatestVersion).FirstOrDefault();

                if (version == null)
                {
                    continue;
                }

                AddPackageToConfig(packageConfig, fileName, version.ToString());
            }

            List<XElement> existingPackages = packageConfig.Root.Elements("package").ToList();

            foreach (XElement packageElement in existingPackages)
            {
                string lowerPackageId = packageElement.Attribute("id").Value.ToLowerInvariant();

                if (IsToolsFile(lowerPackageId))
                {
                    packageElement.SetAttributeValue("developmentDependency", true);
                }
                else
                {
                    packageElement.SetAttributeValue("developmentDependency", false);
                }
            }
        }

        private void AddPackageToConfig(XDocument packageConfig, string packageId, string version)
        {
            if (packageConfig.Root.Elements("package").Any(el => el.Attribute("id").Value == packageId))
            {
                return;
            }

            XElement dependencyElement =
                new XElement("package",
                    new XAttribute("id", packageId),
                    new XAttribute("version", version),
                    new XAttribute("targetFramework", "net461"));

            packageConfig.Root.Add(dependencyElement);
        }

        private void AddReferencesToNuspecFile(Project msbuildProject, XDocument nuspecFile)
        {
            XElement metaDataSection = GetOrAddElement(nuspecFile.Root, "metadata");

            XElement referencesSection = GetOrAddElement(metaDataSection, "references");

            referencesSection.Value = string.Empty;

            ICollection<ProjectItem> refs = msbuildProject.GetItems("ProjectReference");

            if (!refs.Any())
            {
                return;
            }

            XElement groupSection = GetOrAddElement(referencesSection, "group");

            groupSection.RemoveNodes();

            foreach (ProjectItem reference in refs)
            {
                if (ReferenceAssemblyExists(reference))
                {
                    AddReferenceToNuspecFile(groupSection, reference);
                }
            }

            if (!groupSection.Elements().Any())
            {
                groupSection.Remove();
            }
        }

        private bool ReferenceAssemblyExists(ProjectItem reference)
        {
            if (File.Exists(reference.EvaluatedInclude))
            {
                return true;
            }

            AssemblyName refAsm = null;

            try
            {
                refAsm = new AssemblyName(reference.EvaluatedInclude);
            }
            catch
            {
                return false;
            }

            if (!File.Exists(BuildOutputDir.EnsureSuffix("\\") + refAsm.Name)
                && !File.Exists(BuildOutputDir.EnsureSuffix("\\") + refAsm.Name + ".dll")
                && !File.Exists(BuildOutputDir.EnsureSuffix("\\") + refAsm.Name + ".exe"))
            {
                return false;
            }

            return true;
        }

        private void AddReferenceToNuspecFile(XElement nuspecFile, ProjectItem reference)
        {
            AssemblyName refAsm = null;

            if (File.Exists(reference.EvaluatedInclude))
            {
                refAsm = new AssemblyName(Path.GetFileNameWithoutExtension(reference.EvaluatedInclude));
            }
            else
            {
                refAsm = new AssemblyName(reference.EvaluatedInclude);
            }

            XElement referenceElement = new XElement("reference");

            List<string> assemblyNames = new List<string>();

            assemblyNames.Add(BuildOutputDir.EnsureSuffix("\\") + refAsm.Name);

            assemblyNames.Add(BuildOutputDir.EnsureSuffix("\\") + refAsm.Name + ".dll");

            assemblyNames.Add(BuildOutputDir.EnsureSuffix("\\") + refAsm.Name + ".exe");

            string selectedAssembly = assemblyNames.FirstOrDefault(a => File.Exists(a));

            referenceElement.SetAttributeValue("file", Path.GetFileName(selectedAssembly));

            nuspecFile.Add(referenceElement);
        }

        private void RegisterFileDependencies(XDocument nuspecFile)
        {
            string outputDir = BuildOutputDir.EnsureSuffix("\\");

            string[] outputFiles = Directory.GetFiles(outputDir);

            XElement filesSection = GetOrAddElement(nuspecFile.Root, "files");

            filesSection.RemoveNodes();

            foreach (string outputFile in outputFiles)
            {
                string outputFileName = Path.GetFileName(outputFile).ToLowerInvariant();

                if (IsToolsFile(outputFileName))
                {
                    AddFileToNuspec(filesSection, Path.GetFileName(outputFile), "tools");
                }
                else if (IsLibFile(outputFileName))
                {
                    AddFileToNuspec(filesSection, Path.GetFileName(outputFile), "lib");
                }
            }

            if (!filesSection.Elements().Any())
            {
                filesSection.Remove();
            }
        }

        private bool IsLibFile(string outputFileName)
        {
            string lowerFileName = outputFileName.ToLowerInvariant();

            string lowerFileNameWithoutExtension = Path.GetFileNameWithoutExtension(lowerFileName);

            if (Path.GetFileNameWithoutExtension(ProjectFile.ToLowerInvariant()) == Path.GetFileNameWithoutExtension(lowerFileNameWithoutExtension)
                || libFileExtensions.Contains(Path.GetExtension(lowerFileName)))
            {
                return true;
            }

            return false;
        }

        private bool IsToolsFile(string outputFileName)
        {
            string @out = outputFileName.ToLowerInvariant();

            return !IsLibFile(@out);
        }

        private void AddFileToNuspec(XElement filesSection, string fileName, string target)
        {
            XElement fileElement = new XElement("file",
                new XAttribute("src", $"{target}\\" + fileName),
                new XAttribute("target", target));

            filesSection.Add(fileElement);
        }

        private void CopyOutputFilesToProjectDirectory(string projectDir, Func<string, bool> fileSelector)
        {
            string outputDir = BuildOutputDir.EnsureSuffix("\\");
            string projectSubDir = ProjectDirectory.EnsureSuffix("\\") + $"{projectDir}\\";

            string[] outputFiles = Directory.GetFiles(outputDir);

            foreach (string outputFile in outputFiles)
            {
                if (fileSelector(Path.GetFileName(outputFile)))
                {
                    File.Copy(outputFile, projectSubDir + Path.GetFileName(outputFile), true);
                }
            }
        }

        private void SaveXDoc(XDocument packageConfig, string path)
        {
            packageConfig.Save(path);
        }

        private static XElement GetOrAddElement(XElement parent, string elementName)
        {
            XElement element = parent.Element(elementName);

            if (element == null)
            {
                element = new XElement(elementName);

                parent.Add(element);
            }

            return element;
        }
    }
}
namespace CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild
{
    using System.Collections.Generic;
    using System.Linq;
    using Argument;
    using Error;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Result;

    public class MsBuildCompilationTask : AppDomainIsolatedTask
    {
        [Required]
        public string ConfigurationAndPlatform { get; set; }

        [Required]
        public string IntermediateAssembly { get; set; }

        [Required]
        public string CustomConfigurationFile { get; set; }

        [Required]
        public string CustomConfigurationSectionName { get; set; }

        //http://stackoverflow.com/questions/6601502/caching-reflection-data
        [Required]
        public string ProjectFile { get; set; }

        [Required]
        public string SolutionFile { get; set; }

        public string Configuration { get; private set; }

        public string Platform { get; private set; }

        public override bool Execute()
        {
            string[] splittedConfig = ConfigurationAndPlatform.Split('|');

            IPredefinedBuildArgs buildArgs = new MsBuildArgs
                                             {
                                                 Configuration = splittedConfig[0],
                                                 SolutionFile = SolutionFile,
                                                 Platform = splittedConfig[1],
                                                 ProjectFile = ProjectFile
                                             };

            ICompilationResult result = CompilationApi.Compile(buildArgs, CustomConfigurationFile, CustomConfigurationSectionName);

            ProcessResult(result);

            return result.CompilationSucceeded;
        }
        
        protected virtual void ProcessResult(ICompilationResult result)
        {
            if (result.CompilationSucceeded)
            {
                return;
            }

            List<ICompilationError> errors = new List<ICompilationError>();

            List<string> senders = new List<string>();

            result
                .FailedUnitResults
                .ToList()
                .ForEach(step =>
                         {
                             errors.AddRange(step.CompilationErrors);
                             senders.Add(step.UnitConfiguration.Name);
                         });

            foreach (ICompilationError error in errors)
            {
                foreach (DocumentReference location in error.Locations)
                {
                    BuildEngine.LogErrorEvent(
                        new BuildErrorEventArgs(
                            error.Category,
                            error.Code,
                            location.Document,
                            (int) location.TextSelection.StartLine,
                            (int) location.TextSelection.StartColumn,
                            (int) location.TextSelection.EndLine,
                            (int) location.TextSelection.EndColumn,
                            error.Message,
                            string.Empty,
                            string.Join(", ", senders)));
                }
            }
        }
    }
}
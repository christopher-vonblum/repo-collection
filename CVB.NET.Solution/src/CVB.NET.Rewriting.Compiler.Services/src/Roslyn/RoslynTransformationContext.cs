using System;
using System.Collections.Generic;
using CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild;

namespace CVB.NET.Rewriting.Compiler.Services.Roslyn
{
    using System.Linq;
    using Argument;
    using Interfaces.Roslyn;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.MSBuild;

    public class RoslynTransformationContext : IRoslynTransformationContext
    {
        public Solution Solution { get; }

        public Project Project { get; }

        public Compilation Compilation => Project.GetCompilationAsync().Result;

        public RoslynTransformationContext(MsBuildArgs buildArgs)
        {
            try
            {
                MSBuildWorkspace workspace = MSBuildWorkspace.Create(new Dictionary<string, string>
                                                                     {
                                                                         {"Configuration", buildArgs.Configuration},
                                                                         {"Platform", buildArgs.Platform}
                                                                     });

                Project = workspace.OpenProjectAsync(buildArgs.ProjectFile).Result;
            }
            catch (Exception ex)
            {
                ;
            }
        }
    }
}
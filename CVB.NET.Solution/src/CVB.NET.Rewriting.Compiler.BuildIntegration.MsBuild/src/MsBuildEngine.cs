using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild.Project.ProjectWrappers;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Argument;
using CVB.NET.Rewriting.Compiler.Configuration.Models.CompilationUnit;
using CVB.NET.Rewriting.Compiler.Helpers;
using CVB.NET.Rewriting.Compiler.Ioc.Bootstrap;
using CVB.NET.Rewriting.Compiler.Services.Interfaces.MsBuild;
using CVB.NET.Utils.String;

namespace CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild
{
    public class MsBuildEngine : IBuildEngine
    {
        private string[] resolverSearchPaths;
        private IMsBuildProject project;

        public MsBuildEngine(MsBuildProject msBuildProject)
        {
            this.project = msBuildProject;
            resolverSearchPaths = new [] { Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().EscapedCodeBase).LocalPath), project.IntermediateOutputPath, project.OutputPath }
                                  .Concat(msBuildProject.GetReferencedProjects().Select(proj => proj.OutputPath))
                                  .ToArray();
        }

        public IEnumerable<string> GetFileResolverSearchPaths()
        {
            return resolverSearchPaths;
        }

        public IInternalCompilationUnitArgs MakeExecutionUnitArgs(ICompilationUnitConfiguration unitConfiguration)
        {
            CompilationUnitArgs args = new CompilationUnitArgs
            {
                TransformationOutputPath = project.IntermediateOutputPath + unitConfiguration.Name.EnsureSuffix("\\"),
                IntermediatePath = project.IntermediateOutputPath,
                IntermediateAssembly = project.IntermediateAssembly,
                AssemblyName = Assembly.ReflectionOnlyLoadFrom(project.IntermediateAssembly).GetName()
            };

            args.PreTransformationPath = args.TransformationOutputPath + "PreTransform\\";
            args.TransformationOutputAssembly = args.TransformationOutputPath + project.TargetFileName;

            args.CreatePaths();

            return args;
        }
    }
}
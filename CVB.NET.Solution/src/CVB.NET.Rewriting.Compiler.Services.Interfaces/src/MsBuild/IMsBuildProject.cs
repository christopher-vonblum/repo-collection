namespace CVB.NET.Rewriting.Compiler.Services.Interfaces.MsBuild
{
    using System;

    public interface IMsBuildProject
    {
        IMsBuildSolution Solution { get; }
        Guid ProjectGuid { get; }
        string Configuration { get; }
        string Platform { get; }
        string ProjectDir { get; }
        string IntermediateOutputPath { get; }
        string TargetFileName { get; }
        string IntermediateAssembly { get; }
        string OutputPath { get; }
        IMsBuildProject[] GetReferencedProjects();
    }
}
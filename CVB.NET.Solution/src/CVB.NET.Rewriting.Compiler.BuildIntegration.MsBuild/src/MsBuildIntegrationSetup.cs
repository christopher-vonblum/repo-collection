using System;
using CVB.NET.Abstractions.Ioc;
using CVB.NET.Abstractions.Ioc.Registration;
using CVB.NET.Rewriting.Compiler.Argument;
using CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild.Project.ProjectWrappers;
using CVB.NET.Rewriting.Compiler.Ioc.Bootstrap;
using CVB.NET.Rewriting.Compiler.Services.Interfaces.MsBuild;

namespace CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild
{
    public class MsBuildIntegrationSetup : IDependencySetup
    {
        public void InstallInto(IDependencyInstaller dependencyInstaller)
        {
            dependencyInstaller.Register(Service.For<IMsBuildSolution, MsBuildSolution>().Singleton());
            dependencyInstaller.Register(Service.For<IMsBuildProject, MsBuildProject>().Singleton());
            dependencyInstaller.Register(Service.For<IBuildEngine, MsBuildEngine>().Singleton());
        }
    }
}

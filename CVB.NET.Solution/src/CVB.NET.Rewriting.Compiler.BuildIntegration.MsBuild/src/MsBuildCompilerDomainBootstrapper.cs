using CVB.NET.Abstractions.Ioc;
using CVB.NET.Rewriting.Compiler.Ioc.Bootstrap;

namespace CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild
{
    public class MsBuildCompilerDomainBootstrapper : CompilerDomainBootstrapperBase
    {
        protected override IDependencySetup GetBuildIntegrationSetup()
        {
            return new MsBuildIntegrationSetup();
        }
    }
}
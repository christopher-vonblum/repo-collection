using System.Reflection;
using CVB.NET.Abstractions.Ioc;
using CVB.NET.Abstractions.Ioc.Registration;
using CVB.NET.Rewriting.Compiler.BuildIntegration;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Argument;
using CVB.NET.Rewriting.Compiler.Ioc.Bootstrap;
using CVB.NET.Rewriting.Compiler.Services;

namespace CVB.NET.Rewriting.Compiler.Helpers
{
    public class DefaultHelpersSetup : IDependencySetup, ICompilationUnitInitializer
    {
        public void InstallInto(IDependencyInstaller dependencyInstaller)
        {
            dependencyInstaller.Register(Service.For<IIntermediateFileHelper, IntermediateFileHelper>());
            dependencyInstaller.Register(Service.For<IAssemblyDependencyResolver, AssemblyDependencyResolver>().Singleton());
        }
        public void InitializeUnit(IUnitInitializationArgs initializationArgs)
        {
            IAssemblyDependencyResolver resolver = initializationArgs.DependencyResolver.Resolve<IAssemblyDependencyResolver>();
            initializationArgs.DependencyInstaller.Register(Service.For(resolver.ResolveAssembly(initializationArgs.CompilationUnitArgs.AssemblyName)));
        }
    }
}

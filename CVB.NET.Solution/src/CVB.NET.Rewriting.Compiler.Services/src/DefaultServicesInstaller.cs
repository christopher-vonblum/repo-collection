using System.Reflection;
using CVB.NET.Abstractions.Ioc;
using CVB.NET.Abstractions.Ioc.Registration;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Argument;
using CVB.NET.Rewriting.Compiler.Helpers;
using CVB.NET.Rewriting.Compiler.Ioc.Bootstrap;
using CVB.NET.Rewriting.Compiler.Services.Interfaces.Cecil;
using Mono.Cecil;

namespace CVB.NET.Rewriting.Compiler.Services
{
    using Cecil;
    using Interfaces.Roslyn;
    using Ioc;
    using Roslyn;

    public class DefaultServicesInstaller : IDependencySetup, ICompilationUnitInitializer
    {
        public void InstallInto(IDependencyInstaller dependencyInstaller)
        {
            dependencyInstaller.Register(Service.For<IRoslynTransformationContext, RoslynTransformationContext>());
        }

        public void InitializeUnit(IUnitInitializationArgs initializationArgs)
        {
            initializationArgs.DependencyInstaller.Register(Service.For<ICecilAssemblyTransformationContext, CecilAssemblyTransformationContext>());
            initializationArgs.DependencyInstaller.Register(Service.For(initializationArgs.DependencyResolver.Resolve<ICecilAssemblyTransformationContext>().GetTransformationAssembly(initializationArgs.CompilationUnitArgs.AssemblyName)));
        }
    }
}

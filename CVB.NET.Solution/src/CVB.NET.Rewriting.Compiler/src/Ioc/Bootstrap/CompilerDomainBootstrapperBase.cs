using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CVB.NET.Abstractions.Ioc.Registration;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Driver;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Factory.Model;
using CVB.NET.Rewriting.Compiler.Services;
using Unity;
using Unity.Lifetime;

namespace CVB.NET.Rewriting.Compiler.Ioc.Bootstrap
{
    using Abstractions.Adapters.Ioc;
    using Abstractions.Ioc;
    using CompilationUnit.Factory;
    using Configuration;
    using Helpers;
    using Service;
    using Service = Abstractions.Ioc.Registration.Service;

    public abstract class CompilerDomainBootstrapperBase : ICompilerDomainBootstrapper
    {
        private IUnityContainer unityContainer;
        private UnityDependencyService dependencyService;

        public CompilerDomainBootstrapperBase()
        {
            unityContainer = new UnityContainer();
            dependencyService = new UnityDependencyService(unityContainer);
        }

        public virtual IDependencyService CreateDependencyService()
        {
            return dependencyService;
        }

        public virtual IDependencyInjectionProvider CreateDependencyInjectionProvider()
        {
            return dependencyService;
        }
        public virtual void Bootstrap()
        {
            unityContainer.RegisterInstance(dependencyService, new HierarchicalLifetimeManager());
            unityContainer.RegisterInstance<IDependencyInjectionProvider>(dependencyService, new HierarchicalLifetimeManager());
            unityContainer.RegisterInstance<IDependencyInstaller>(dependencyService, new HierarchicalLifetimeManager());
            unityContainer.RegisterInstance<IDependencyResolver>(dependencyService, new HierarchicalLifetimeManager());

            dependencyService.Register(Service.For<IDependencyService, UnityDependencyService>());

            dependencyService.Install<DefaultHelpersSetup>();

            dependencyService.Install(GetBuildIntegrationSetup());

            dependencyService.Resolve<IAssemblyDependencyResolver>();

            dependencyService.Register(Service.For<ICompilerConfigurationRepository, CompilerConfigurationRepository>().Singleton());

            dependencyService.Register(Service.For<ICompiler, Compiler>().Singleton());
            dependencyService.Register(Service.For<ICompilationUnitDriver, CompilationUnitDriver>().Singleton());
            dependencyService.Register(Service.For<ICompilationUnitRunner, CompilationUnitRunner>().Transient());

            dependencyService.Register(Service.For<ICompilationUnitFactory, CompilationUnitFactory>().Transient());

            RunServiceAssemblyInstallers(dependencyService, dependencyService.Resolve<ICompilerConfigurationRepository>());
        }

        protected abstract IDependencySetup GetBuildIntegrationSetup();

        private static void RunServiceAssemblyInstallers(IDependencyService dependencyService, ICompilerConfigurationRepository configurationRepository)
        {
            Assembly[] assemblies = configurationRepository.GetServiceAssemblies();

            List<ICompilationUnitInitializer> initializers = new List<ICompilationUnitInitializer>();

            assemblies
                .SelectMany(assembly =>
                                assembly
                                    .GetCustomAttributes(typeof(ServiceAssemblyAttribute), false)
                                    .OfType<ServiceAssemblyAttribute>())
                .ToList()
                .ForEach(attr =>
                             attr
                                 .InstallerTypes
                                 .ToList()
                                 .ForEach(t =>
                                          {

                                              IDependencySetup setup = (IDependencySetup) dependencyService.Resolve(t);
                                              setup.InstallInto(dependencyService);

                                              if (setup is ICompilationUnitInitializer)
                                              {
                                                  initializers.Add((ICompilationUnitInitializer)setup);
                                              }
                                          }));

            dependencyService.Register(Service.For<ICompilationUnitFactoryConfiguration>(new CompilationUnitFactoryConfiguration { Initializers = initializers }));
        }
    }
}
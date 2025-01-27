using System;

using CVB.NET.Abstractions.Ioc;
using CVB.NET.Abstractions.Ioc.Registration;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Argument;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Factory.Model;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Step;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Task;
using CVB.NET.Rewriting.Compiler.Ioc.Bootstrap;
using CVB.NET.Rewriting.Compiler.Services;

namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Factory
{
    using System.Linq;
    using Configuration.Models.Attributes;
    using Configuration.Models.CompilationUnit;

    using Reflection.Caching;
    using Reflection.Caching.Cached;

    public class CompilationUnitFactory : ICompilationUnitFactory
    {
        private readonly IDependencyService dependencyService;
        private readonly ICompilationUnitFactoryConfiguration configuration;

        public CompilationUnitFactory(IDependencyService dependencyService, ICompilationUnitFactoryConfiguration configuration)
        {
            this.dependencyService = dependencyService;
            this.configuration = configuration;
        }

        public ICompilationUnit CreateUnit(ICompilationUnitConfiguration unitConfiguration)
        {
            if (unitConfiguration.ConfigurationImplementationType != null)
            {
                CachedType configurationImplementationType = unitConfiguration.ConfigurationImplementationType;

                if (configurationImplementationType.InnerReflectionInfo.IsInterface)
                {
                    configurationImplementationType = ReflectionCache
                        .Get<CachedType>(unitConfiguration.ConfigurationImplementationType)
                        .Attributes
                        .OfType<ConfigurationInterfaceImplementationAttribute>()
                        .First()
                        .ImplementationType;
                }

                ICompilationUnitConfiguration typedConfiguration = (ICompilationUnitConfiguration)configurationImplementationType
                                                                                .DefaultConstructor
                                                                                .InnerReflectionInfo
                                                                                .Invoke(null);

                typedConfiguration.ProxyTarget = unitConfiguration;
                unitConfiguration = typedConfiguration;
            }

            IUnitInitializationArgs initializationArgs = new UnitInitializationArgs
                                                         {
                                                             DependencyInstaller = dependencyService,
                                                             CompilationUnitArgs = dependencyService.Resolve<ICompilationUnitArgs>(),
                                                             DependencyResolver = dependencyService
                                                         };

            foreach (ICompilationUnitInitializer compilationUnitInitializer in configuration.Initializers)
            {
                compilationUnitInitializer.InitializeUnit(initializationArgs);
            }

            ICompilationUnit unitInstance = ResolveCompilationUnit(unitConfiguration.ImplementationType, unitConfiguration.Name);

            unitInstance.Configure(unitConfiguration);

            return unitInstance;
        }

        public ICompilationUnit ResolveCompilationUnit(Type unitType, string name)
        {
            if (typeof(ICompilationStep).IsAssignableFrom(unitType))
            {
                return this.ResolveSpecificUnit(unitType, name, "step_");
            }

            if (typeof(ICompilationTask).IsAssignableFrom(unitType))
            {
                return this.ResolveSpecificUnit(unitType, name, "task_");
            }

            throw new Exception("nope");
        }

        private ICompilationUnit ResolveSpecificUnit(Type unitType, string name, string iocPrefix)
        {
            string unitName = iocPrefix + name;

            bool registerUnit = !this.dependencyService.HasRegistration(unitType, unitName);

            if (registerUnit)
            {
                this.dependencyService.Register(Service.For(unitType).Named(unitName).Transient());
            }

            return (ICompilationUnit)this.dependencyService.Resolve(unitType, unitName);
        }
    }
}
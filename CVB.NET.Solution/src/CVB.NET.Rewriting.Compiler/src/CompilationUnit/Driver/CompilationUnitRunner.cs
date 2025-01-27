using System;
using CVB.NET.Abstractions.Adapters.Ioc;
using CVB.NET.Abstractions.Ioc;
using CVB.NET.Abstractions.Ioc.Registration;
using CVB.NET.Rewriting.Compiler.Argument;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Argument;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Factory;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Result;
using CVB.NET.Rewriting.Compiler.Configuration;
using CVB.NET.Rewriting.Compiler.Configuration.Models;
using CVB.NET.Rewriting.Compiler.Configuration.Models.CompilationUnit;
using CVB.NET.Rewriting.Compiler.Error;
using Unity;
using Unity.Attributes;
using Unity.Exceptions;

namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Driver
{
    public class CompilationUnitRunner : MarshalByRefObject, ICompilationUnitRunner
    {
        private ICompilerConfigurationRepository executionUnitConfigurationRepository;
        private IUnityContainer container;

        [InjectionMethod]
        public void Inject(IUnityContainer container, ICompilerConfigurationRepository executionUnitConfigurationRepository)
        {
            this.container = container;
            this.executionUnitConfigurationRepository = executionUnitConfigurationRepository;
        }

        public ICompilationUnitResult Evaluate(ICompilationUnitArgs args, ICompilationUnitConfiguration unitConfiguration)
        {
            IUnityContainer unitChildContainer = this.container.CreateChildContainer();

            IDependencyService unitDependencyService = unitChildContainer.Resolve<IDependencyService>();

            unitChildContainer.RegisterInstance(unitDependencyService);

            unitDependencyService.Register(Service.For(args));
            unitDependencyService.Register(Service.For((IInternalCompilationUnitArgs)args));

            ICompilationUnit executionUnit;

            ICompilationUnitFactory factory = unitDependencyService.Resolve<ICompilationUnitFactory>();

            try
            {
                executionUnit = factory.CreateUnit(unitConfiguration);
            }
            catch (ResolutionFailedException ex)
            {
                return new CompilationUnitResult
                       {
                           InitializationFailed = true,
                           CompilationErrors = CompilationError.FromException(ex.InnerException)
                       };
            }

            var result = executionUnit.Invoke(args);

            unitDependencyService.Teardown();

            return result;
        }

        // TODO: one day we should pass the configuration in directly without the repository
        public ICompilationUnitResult EvaluateRemote(ICompilationUnitArgs args, string unitName)
        {
            return Evaluate(args, this.executionUnitConfigurationRepository.GetTaskConfiguration(unitName));
        }

        public void BootstrapRunnerDomain(IPredefinedBuildArgs args, ICompilerConfiguration configuration)
        {
            CompilationApi.BootstrapDomainBootstrapper(args, configuration).Resolve<IDependencyInjectionProvider>().InjectDependencies(this);
        }
    }
}

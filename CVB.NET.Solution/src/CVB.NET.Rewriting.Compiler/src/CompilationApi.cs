using System.Configuration;

using CVB.NET.Abstractions.Ioc;
using CVB.NET.Abstractions.Ioc.Registration;
using CVB.NET.Rewriting.Compiler.Configuration.Models;
using CVB.NET.Rewriting.Compiler.Ioc.Bootstrap;

namespace CVB.NET.Rewriting.Compiler
{
    using Argument;
    using Result;

    public static class CompilationApi
    {
        internal static IDependencyService BootstrapDomainBootstrapper(IPredefinedBuildArgs args, ICompilerConfiguration configuration)
        {
            ICompilerDomainBootstrapper bootstrapper = (ICompilerDomainBootstrapper)(args.CompilerDomainBootstrapperType ?? typeof(CompilerDomainBootstrapperBase)).DefaultConstructor.InnerReflectionInfo.Invoke(null);

            IDependencyService compilationContainer = bootstrapper.CreateDependencyService();

            compilationContainer.Register(Service.For(configuration));

            compilationContainer.Register(Service.For(args));

            compilationContainer.Register(Service.For(args.GetType(), args));

            bootstrapper.Bootstrap();
            
            return compilationContainer;
        }

        private static ICompilationResult CompileInternal(IPredefinedBuildArgs args, ICompilerConfiguration configuration)
        {
            ICompilationResult result;

            IDependencyService compilationContainer = BootstrapDomainBootstrapper(args, configuration);
            
            ICompiler compiler = compilationContainer.Resolve<ICompiler>();

            result = compiler.Compile(args);
            
            compilationContainer.Teardown();

            return result;
        }

        public static ICompilationResult Compile(IPredefinedBuildArgs args, ICompilerConfiguration configuration)
        {
            return CompileInternal(args, configuration);
        }

        public static ICompilationResult Compile(IPredefinedBuildArgs args, string configurationFile, string configurationSectionName)
        {
            return CompileInternal(args, LoadConfigurationFile(configurationFile, configurationSectionName));
        }

        private static ICompilerConfiguration LoadConfigurationFile(string configurationFile, string configurationSectionName)
        {
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap { ExeConfigFilename = configurationFile };

            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            
            return (ICompilerConfiguration)config.GetSection(configurationSectionName);
        }
    }
}

using System;
using System.IO;
using CVB.NET.Rewriting.Compiler.Configuration.Exception;

namespace CVB.NET.Rewriting.Compiler.Configuration
{
    using System.Linq;
    using System.Reflection;

    using Models;
    using Models.CompilationUnit.Step;
    using Models.CompilationUnit.Task;

    public class CompilerConfigurationRepository : ICompilerConfigurationRepository
    {
        private readonly ICompilerConfiguration configuration;

        public CompilerConfigurationRepository(ICompilerConfiguration configuration)
        {
            this.configuration = configuration;
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => this.ResolveServiceAssembly(new AssemblyName(args.Name));
        }

        private Assembly ResolveServiceAssembly(AssemblyName assemblyName)
        {
            string[] assemblies =
                Directory
                    .GetFiles(Path.GetDirectoryName(new Uri(typeof(CompilerConfigurationRepository).Assembly.EscapedCodeBase).LocalPath))
                    .Where(
                        path =>
                            Path.GetExtension(path).ToLowerInvariant().Equals(".exe")
                            || Path.GetExtension(path).ToLowerInvariant().Equals(".dll"))
                    .ToArray();

            foreach (string assem in assemblies)
            {
                try
                {
                    Assembly reflectionOnlyAssem = Assembly.ReflectionOnlyLoadFrom(assem);

                    if (reflectionOnlyAssem.GetName().FullName.Equals(assemblyName.FullName))
                    {
                        try
                        {
                            return Assembly.LoadFile(assem);
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
            }

            return null;
        }

        public Assembly[] GetServiceAssemblies()
        {
            return configuration.ServiceAssemblies.Select(asm => Assembly.Load(asm.FullName)).ToArray();
        }

        public ICompilationStepConfiguration[] GetAllStepConfigurations()
        {
            return configuration.CompilationSteps.ToArray();
        }
        public ICompilationTaskConfiguration GetTaskConfiguration(string name)
        {
            return configuration.CompilationTasks.FirstOrDefault(t => t.Name.Equals(name)) ?? throw new CompilationTaskNotFoundException(name);
        }
    }
}